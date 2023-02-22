using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.GitVersion;

public partial class Build
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [GitVersion] readonly GitVersion GitVersion;
    [GitRepository] readonly GitRepository GitRepository;
    [Solution] readonly Solution Solution;

    const string ArtifactsFolder = "output";
    readonly AbsolutePath ArtifactsDirectory = RootDirectory / ArtifactsFolder;

    const string Author = "Coolicky";
    const string Description = "Set of Wpf utilities mostly for Revit and Navisworks development";
    readonly string[] Tags = { "revit", "navisworks", "wpf", "toolkit" };
    const string RepoType = "git";
    const string PackageIcon = ".nuget/PackageIcon.png";
    const string LicenceFile = "License.md";
}