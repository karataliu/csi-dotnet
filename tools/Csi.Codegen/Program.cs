using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Csi.Codegen
{
  class Program
  {
    static void Main(string[] args)
    {
      var tmpRoot = Path.Combine(Path.GetTempPath(), "csi-dotnet-tools");
      var protoDir = Path.Combine(tmpRoot, "p2");
      var cpd = new CsiProtoDownload(protoDir);
      var version = "v0.2.0";
      cpd.GetVersion(version).Wait();

      var ptRoot = Path.Combine(protoDir, "spec-" + version.TrimStart('v'));

      var installer = new GrpcToolsWrapper(Path.Combine(tmpRoot, "grpc-csi"));
      installer.Prepare().Wait();
      installer.Generate(Path.Combine(ptRoot, "csi.proto"), ptRoot);

    }
  }

  class CsiProtoDownload
  {
    private readonly string workspace;
    public CsiProtoDownload(string workspace)
    {
      this.workspace = workspace;
    }
    public Task GetVersion(string version)
      => Util.ExtractZipToDir($"https://github.com/container-storage-interface/spec/archive/{version}.zip", workspace);
  }

  class GrpcToolsWrapper
  {
    private const string url = "https://www.nuget.org/api/v2/package/Grpc.Tools/1.11.0";

    private readonly string workspace;
    private string protocPath;
    private string grpcPluginPath;

    public GrpcToolsWrapper(string workspace)
    {
      this.workspace = workspace;
    }

    public async Task Prepare()
    {
      var webClient = new WebClient();

      Directory.CreateDirectory(workspace);
      var zip = Path.Combine(workspace, "g.zip");
      if (!File.Exists(zip)) await webClient.DownloadFileTaskAsync(new Uri(url), zip);

      var destDir = Path.Combine(workspace, "out");
      if (!Directory.Exists(destDir)) ZipFile.ExtractToDirectory(zip, destDir);

      // check os
      protocPath = Path.Combine(destDir, "tools", "linux_x64", "protoc");
      grpcPluginPath = Path.Combine(destDir, "tools", "linux_x64", "grpc_csharp_plugin");
      Util.RunCmd("chmod", "+x", protocPath, grpcPluginPath);
    }

    public void Generate(string protoFile, string includeDir)
    {
      var outTmp = Path.Combine(workspace, "csharp-out");
      Directory.CreateDirectory(outTmp);
      Util.RunCmd(protocPath,
         $"--plugin=protoc-gen-grpc={grpcPluginPath}",
         "--csharp_out", outTmp,
         "--grpc_out", outTmp,
         "-I", includeDir,
         protoFile
      );
      System.Console.WriteLine(outTmp);
    }
  }
}
