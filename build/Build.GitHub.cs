using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitHub;
using Nuke.Common.Tools.GitVersion;
using Octokit;
using Serilog;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

public partial class Build
{
    const string NugetGithubUrl = "https://nuget.pkg.github.com/Coolicky/index.json";
    [GitVersion] readonly GitVersion GitVersion;
    [GitRepository] readonly GitRepository GitRepository;
    [Secret] [Parameter] string GitHubToken;

    Target CompileTest => _ => _
        .Requires(() => GitRepository)
        .OnlyWhenStatic(() => !GitRepository.IsOnMainOrMasterBranch())
        .DependsOn(Compile);

    Target NuGetPushGitHub => _ => _
        .Requires(() => GitHubToken)
        .Requires(() => GitRepository)
        .DependsOn(Pack)
        .OnlyWhenStatic(() => GitRepository.IsOnMainOrMasterBranch())
        .Executes(() =>
        {
            var package = GetNugetPackage();
            if (package is null) return;
            DotNetNuGetPush(settings => settings
                .SetTargetPath(package)
                .SetSource(NugetGithubUrl)
                .SetApiKey(GitHubToken));
        });

    Target GitHubRelease => _ => _
        .TriggeredBy(NuGetPushGitHub)
        .Requires(() => GitHubToken)
        .Requires(() => GitRepository)
        .OnlyWhenStatic(() => GitRepository.IsOnMainOrMasterBranch())
        .OnlyWhenStatic(() => IsServerBuild)
        .Executes(async () =>
        {
            GitHubTasks.GitHubClient = new GitHubClient(new ProductHeaderValue(Solution.Name))
            {
                Credentials = new Credentials(GitHubToken)
            };

            var (owner, name) = (GitRepository.GetGitHubOwner(), GitRepository.GetGitHubName());
            var releaseTag = GitVersion.NuGetVersionV2;

            var newRelease = new NewRelease(releaseTag)
            {
                TargetCommitish = GitVersion.Sha,
                Draft = true,
                Name = $"v{releaseTag}",
                Prerelease = !string.IsNullOrEmpty(GitVersion.PreReleaseTag),
            };
            var release = await GitHubTasks.GitHubClient.Repository.Release.Create(owner, name, newRelease);
            var files = RootDirectory
                .GlobFiles("**/*.nupkg")
                .ToArray();

            await UploadReleaseToGithub(release, files);

            await GitHubTasks.GitHubClient
                .Repository
                .Release
                .Edit(owner, name, release.Id, new ReleaseUpdate { Draft = false });
        });

    static async Task UploadReleaseToGithub(Release release, IEnumerable<AbsolutePath> file)
    {
        foreach (var absolutePath in file)
        {
            await using var stream = File.OpenRead(absolutePath.ToString());
            if (!absolutePath.FileExists())
            {
                Log.Information("File does not exist: {Path}", absolutePath.ToString());
                return;
            }

            var asset = new ReleaseAssetUpload
            {
                ContentType = "application/x-binary",
                FileName = Path.GetFileName(absolutePath),
                RawData = File.OpenRead(absolutePath)
            };
            await GitHubTasks.GitHubClient.Repository.Release.UploadAsset(release, asset);
            Log.Information("Added artifact: {Path}", absolutePath);
        }
    }
}