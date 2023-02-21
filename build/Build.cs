using System.Linq;
using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("Version")] readonly string Version;

    [GitRepository] readonly GitRepository GitRepository;
    [Solution] readonly Solution Solution;

    const string ArtifactsFolder = "output";
    readonly AbsolutePath ArtifactsDirectory = RootDirectory / ArtifactsFolder;


    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore();
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(r => r
                .SetConfiguration(Configuration)
                .SetVerbosity(DotNetVerbosity.Minimal));
        });

    Target Pack => _ => _
        .TriggeredBy(Compile)
        .Requires(() => Version)
        .Executes(() =>
        {
            DotNetPack(settings => settings
                .SetConfiguration(Configuration)
                .SetVersion(Version)
                .SetOutputDirectory(ArtifactsDirectory)
                .SetVerbosity(DotNetVerbosity.Minimal));
        });
}

partial class Build
{
    const string NugetApiUrl = "https://api.nuget.org/v3/index.json";
    [Secret] [Parameter] string NugetApiKey;

    Target NuGetPush => _ => _
        .Requires(() => NugetApiKey)
        .DependsOn(Pack)
        .OnlyWhenStatic(() => GitRepository.IsOnMainOrMasterBranch())
        .OnlyWhenStatic(() => IsLocalBuild)
        .Executes(() =>
        {
            var package = ArtifactsDirectory
                .GlobFiles("*.nupkg")
                .FirstOrDefault(r =>
                    r.NameWithoutExtension.EndsWith(Version));
            if (package is null) return;
            DotNetNuGetPush(settings => settings
                .SetTargetPath(package)
                .SetSource(NugetApiUrl)
                .SetApiKey(NugetApiKey));
        });
}