using System;
using System.Collections.Generic;
using System.IO;
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

    const string riderVersion = "1.2.8.33";

    AbsolutePath ArtifactsDirectory => RootDirectory / "packages";
    AbsolutePath RiderZip => RootDirectory / "Rider" / "zip";
    AbsolutePath RiderDir => RiderZip / "AsyncConverter.Rider";
    AbsolutePath RiderJarDir => RiderDir / "lib";
    AbsolutePath RiderDotnetDir => RiderDir / "dotnet";
    AbsolutePath RiderMetaDir => RootDirectory / "Rider" / "meta";
    AbsolutePath RiderBinDir => RootDirectory / "AsyncConverter" / "bin" / "Rider" / Configuration / "net472";

    Target Clean => _ => _
                        .Before(Restore)
                        .Executes(() =>
                                  {
                                      DotNetClean();
                                      EnsureCleanDirectory(ArtifactsDirectory);
                                      DeleteFilesInDir(RiderJarDir);
                                      DeleteFilesInDir(RiderDotnetDir);
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
                      DotNetBuild(c => DotNetBuildSettingsExtensions.SetConfiguration(c, Configuration));
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
        ZipFile.CreateFromDirectory(RiderMetaDir,
            RiderJarDir / $"AsyncConverter.Rider-{riderVersion}.jar");
        File.Copy(RiderBinDir / "AsyncConverter.Rider.dll",
            RiderDotnetDir / "AsyncConverter.Rider.dll", true);
        ZipFile.CreateFromDirectory(RiderZip, ArtifactsDirectory / $"AsyncConverter.Rider.zip");
    }

    void PackForReSharper() =>
        DotNetPack(s =>
                   {
                       var settings = DotNetPackSettingsExtensions.SetOutputDirectory(s, ArtifactsDirectory).DisableIncludeSymbols();
                       return DotNetPackSettingsExtensions.SetProject(settings, "AsyncConverter/AsyncConverter.csproj");
                   });

    void DeleteFilesInDir(AbsolutePath dir)
    {
        if(!Directory.Exists(dir))
            return;
        foreach (var file in Directory.GetFiles(dir))
        {
            File.Delete(file);
        }
    }

}
