using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;

public partial class Build
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
    
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