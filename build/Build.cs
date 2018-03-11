using System.IO;
using Nuke.Common.Tools.DotNet;
using Nuke.Core;
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

    Target Clean => _ => _
            .OnlyWhen(() => false) // Disabled for safety.
            .Executes(() =>
            {
                DeleteDirectories(GlobDirectories(SourceDirectory, "**/bin", "**/obj"));
                EnsureCleanDirectory(OutputDirectory);
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
                                        DotNetPack(s => DefaultDotNetPack
                                                       .SetOutputDirectory(Path.Combine(SolutionDirectory, "packages"))
                                                       .DisableIncludeSymbols()
                                                       .SetProject("AsyncConverter/AsyncConverter.csproj"));

                                        DotNetPack(s => DefaultDotNetPack
                                                       .SetOutputDirectory(Path.Combine(SolutionDirectory, "Rider"))
                                                       .DisableIncludeSymbols()
                                                       .SetProject("AsyncConverter/AsyncConverter.Rider.csproj"));
                                    });
}
