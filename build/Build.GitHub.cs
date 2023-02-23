using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Git;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[GitHubActions("NugetPush",
    GitHubActionsImage.UbuntuLatest,
    On = new[] { GitHubActionsTrigger.Push },
    InvokedTargets = new[] { nameof(NuGetPushGithub) },
    EnableGitHubToken = true)]
public partial class Build
{
    Target NuGetPushGithub => _ => _
        // .Requires(() => GithubToken)
        .DependsOn(Pack)
        .OnlyWhenStatic(() => GitRepository.IsOnMainOrMasterBranch())
        .Executes(() =>
        {
            var package = GetNugetPackage();
            if (package is null) return;
            DotNetNuGetPush(settings => settings
                .SetTargetPath(package)
                .SetSource(NugetGithubUrl)
                .SetApiKey(GithubToken));
        });
}