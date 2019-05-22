using System.IO;
using System.IO.Compression;
using Nuke.Common.Tools.DotNet;
using Nuke.Core;
using Nuke.Core.Utilities.Collections;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Core.IO.FileSystemTasks;
using static Nuke.Core.IO.PathConstruction;

class Build : NukeBuild
{
    // Console application entry. Also defines the default target.
    public static int Main () => Execute<Build>(x => x.Compile);

    // Auto-injection fields:

    // [GitVersion] readonly GitVersion GitVersion;
    // Semantic versioning. Must have 'GitVersion.CommandLine' referenced.

    // [GitRepository] readonly GitRepository GitRepository;
    // Parses origin, branch name and head from git config.

    // [Parameter] readonly string MyGetApiKey;
    // Returns command-line arguments and environment variables.

    public override AbsolutePath ArtifactsDirectory => SolutionDirectory / "packages";

    Target Clean => _ => _
            .Executes(() =>
            {
                EnsureCleanDirectory(ArtifactsDirectory);
                var directories = GlobDirectories(SolutionDirectory / "AsyncConverter", "**/bin", "**/obj");
                directories.ForEach(EnsureCleanDirectory);
            });

    Target Restore => _ => _
            .DependsOn(Clean)
            .Executes(() =>
            {
                DotNetRestore(s => DefaultDotNetRestore.SetProjectFile("AsyncConverter/AsyncConverter.csproj"));
                DotNetRestore(s => DefaultDotNetRestore.SetProjectFile("AsyncConverter/AsyncConverter.Rider.csproj"));
                DotNetRestore(s => DefaultDotNetRestore.SetProjectFile("AsyncConverter.Tests/AsyncConverter.Tests.csproj"));
                DotNetRestore(s => DefaultDotNetRestore.SetProjectFile("AsyncConverter.Tests/AsyncConverter.Rider.Tests.csproj"));
            });

    Target Compile => _ => _
            .DependsOn(Restore)
            .Executes(() =>
            {
                DotNetBuild(s => DefaultDotNetBuild);
            });

    Target Pack => _ => _
                          .DependsOn(Compile)
                          .Executes(() =>
                                    {
                                    //TODO: DeleteDirectory not work, and move to Clean
                                        if (Directory.Exists(ArtifactsDirectory))
                                            Directory.Delete(ArtifactsDirectory, true);

                                        DotNetPack(s => DefaultDotNetPack
                                                       .SetOutputDirectory(ArtifactsDirectory)
                                                       .DisableIncludeSymbols()
                                                       .SetProject("AsyncConverter/AsyncConverter.csproj"));

                                        DotNetPack(s => DefaultDotNetPack
                                                       .SetOutputDirectory(SolutionDirectory / "Rider" / "AsyncConverter.Rider")
                                                       .DisableIncludeSymbols()
                                                       .SetProject("AsyncConverter/AsyncConverter.Rider.csproj"));

                                        ZipFile.CreateFromDirectory(SolutionDirectory / "Rider",
                                            ArtifactsDirectory / $"AsyncConverter.Rider.zip");
                                    });
}
