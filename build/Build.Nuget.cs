using System.Linq;
using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

public partial class Build
{
    const string NugetApiUrl = "https://api.nuget.org/v3/index.json";
    [Secret] [Parameter] string NugetApiKey;
    
    const string NugetGithubUrl = "https://nuget.pkg.github.com/Coolicky/index.json";
    [Secret] [Parameter] string GithubToken;

    Target NuGetPush => _ => _
        .Requires(() => GithubToken)
        .DependsOn(Pack)
        .OnlyWhenStatic(() => GitRepository.IsOnMainOrMasterBranch())
        .OnlyWhenStatic(() => IsLocalBuild)
        .Executes(() =>
        {
            var package = GetNugetPackage();
            if (package is null) return;
            DotNetNuGetPush(settings => settings
                .SetTargetPath(package)
                .SetSource(NugetApiUrl)
                .SetApiKey(NugetApiKey));
        });
    
    Target NuGetPushGithub => _ => _
        .Requires(() => GithubToken)
        .DependsOn(Pack)
        .OnlyWhenStatic(() => GitRepository.IsOnMainOrMasterBranch())
        .OnlyWhenStatic(() => IsLocalBuild)
        .Executes(() =>
        {
            var package = GetNugetPackage();
            if (package is null) return;
            DotNetNuGetPush(settings => settings
                .SetTargetPath(package)
                .SetSource(NugetGithubUrl)
                .SetApiKey(GithubToken));
        });

    AbsolutePath GetNugetPackage()
    {
        //TODO: Set properly when microsoft fixes the issue
        var package = RootDirectory
            .GlobFiles("**/*.nupkg")
            .FirstOrDefault(r =>
                r.NameWithoutExtension.EndsWith(GitVersion.SemVer));
        return package;
    }
}