using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Nuke.Common;
using Nuke.Common.IO;
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

    readonly string WaveVersion = "223";
    readonly string SdkVersion = "2022.3.1";
    readonly string PluginVersion = "36";

    string ResharperVersion => $"1.1.8.{PluginVersion}";
    string RiderVersion => $"1.2.8.{PluginVersion}";

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
                          .DependsOn(UpdateVersion)
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
    Target UpdateVersion => _ => _
                       .Executes(() =>
                                 {
                                     PatchResharper();
                                     PatchRider();
                                 });

    void PatchRider()
    {
        PatchFile(RiderMetaDir / "META-INF" / "plugin.xml",
            new List<(string, string)>
            {
                ("<version>\\d+\\.\\d+\\.\\d+\\.\\d+</version>", $"<version>{RiderVersion}</version>"),
                ("<idea-version since-build=\"\\d+\" until-build=\"\\d+\\.\\*\"/>",
                    $"<idea-version since-build=\"{WaveVersion}\" until-build=\"{WaveVersion}.*\"/>")
            }, Encoding.ASCII);
        PatchFile(RootDirectory / "AsyncConverter" / "AsyncConverter.Rider.csproj",
            new List<(string, string)>
            {
                ("<Version>\\d+\\.\\d+\\.\\d+\\.\\d+</Version>", $"<Version>{RiderVersion}</Version>"),
                ("<PackageReference Include=\"Wave\" Version=\"\\[\\d+\\]\" />", $"<PackageReference Include=\"Wave\" Version=\"[{WaveVersion}]\" />"),
                ("<PackageReference Include=\"JetBrains.Rider.SDK\" Version=\"\\d+\\.\\d+\\.\\d+\" PrivateAssets=\"All\" />",
                    $"<PackageReference Include=\"JetBrains.Rider.SDK\" Version=\"{SdkVersion}\" PrivateAssets=\"All\" />")
            });
        PatchFile(RootDirectory / "AsyncConverter.Tests" / "AsyncConverter.Rider.Tests.csproj",
            new List<(string pattern, string replacement)>
            {
                ("<PackageReference Include=\"JetBrains.Rider.SDK.Tests\" Version=\"\\d+\\.\\d+\\.\\d+\" />",
                    $"<PackageReference Include=\"JetBrains.Rider.SDK.Tests\" Version=\"{SdkVersion}\" />"),
            });
    }

    void PatchResharper()
    {
        PatchFile(RootDirectory / "AsyncConverter" / "AsyncConverter.csproj",
            new List<(string, string)>
            {
                ("<Version>\\d+\\.\\d+\\.\\d+\\.\\d+</Version>", $"<Version>{ResharperVersion}</Version>"),
                ("<PackageReference Include=\"Wave\" Version=\"\\[\\d+\\]\" />", $"<PackageReference Include=\"Wave\" Version=\"[{WaveVersion}]\" />"),
                ("<PackageReference Include=\"JetBrains.ReSharper.SDK\" Version=\"\\d+\\.\\d+\\.\\d+\" PrivateAssets=\"All\" />",
                    $"<PackageReference Include=\"JetBrains.ReSharper.SDK\" Version=\"{SdkVersion}\" PrivateAssets=\"All\" />")
            });
        PatchFile(RootDirectory / "AsyncConverter.Tests" / "AsyncConverter.Tests.csproj",
            new List<(string pattern, string replacement)>
            {
                ("<PackageReference Include=\"JetBrains.ReSharper.SDK.Tests\" Version=\"\\d+\\.\\d+\\.\\d+\" />",
                    $"<PackageReference Include=\"JetBrains.ReSharper.SDK.Tests\" Version=\"{SdkVersion}\" />"),
            });
    }

    void PatchFile(AbsolutePath filePath, List<(string pattern, string replacement)> replaces, Encoding encoding = null)
    {
        var content = File.ReadAllText(filePath);
        content = replaces
            .Aggregate(content, (current, replace) => Regex.Replace(current, replace.pattern, replace.replacement));
        File.WriteAllText(filePath, content, encoding ?? Encoding.UTF8);
    }

    void PackForRider()
    {
        ZipFile.CreateFromDirectory(RiderMetaDir,
            RiderJarDir / $"AsyncConverter.Rider.jar");
        File.Copy(RiderBinDir / "AsyncConverter.Rider.dll",
            RiderDotnetDir / "AsyncConverter.Rider.dll", true);
        ZipFile.CreateFromDirectory(RiderZip, ArtifactsDirectory / $"AsyncConverter.Rider.zip");
    }

    void PackForReSharper() =>
        DotNetPack(s =>
                   {
                       s = DotNetPackSettingsExtensions.SetOutputDirectory(s, ArtifactsDirectory).DisableIncludeSymbols();
                       return DotNetPackSettingsExtensions.SetProject(s, "AsyncConverter/AsyncConverter.csproj");
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
