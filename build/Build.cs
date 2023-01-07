using System.Collections.Generic;
using System.IO.Compression;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    public AbsolutePath ArtifactsDirectory => RootDirectory / "packages";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
                  {
                      DotNetClean();
                      EnsureCleanDirectory(ArtifactsDirectory);
                  });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
                  {
                      DotNetRestore();
                  });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
                  {
                      DotNetBuild();
                  });
    Target Pack => _ => _
       .DependsOn(Compile)
       .Executes(() =>
                 {
                     PackForReSharper();
                     PackForRider();
                 });

    void PackForRider()
    {
        DotNetPack(s =>
                   {
                       var settings = DotNetPackSettingsExtensions.SetOutputDirectory(s, RootDirectory / "Rider" / "AsyncConverter.Rider").DisableIncludeSymbols();
                       return DotNetPackSettingsExtensions.SetProject(settings, "AsyncConverter/AsyncConverter.Rider.csproj");
                   });
        ZipFile.CreateFromDirectory(RootDirectory / "Rider", ArtifactsDirectory / $"AsyncConverter.Rider.zip");
    }

    void PackForReSharper() =>
        DotNetPack(s =>
                   {
                       var settings = DotNetPackSettingsExtensions.SetOutputDirectory(s, ArtifactsDirectory).DisableIncludeSymbols();
                       return DotNetPackSettingsExtensions.SetProject(settings, "AsyncConverter/AsyncConverter.csproj");
                   });
}
