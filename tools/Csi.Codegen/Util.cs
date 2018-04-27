using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Csi.Codegen
{
  class Util
  {
    public static async Task ExtractZipToDir(string url, string dest)
    {
      if (!Directory.Exists(dest))
      {
        var tmp = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tmp);
        var zip = Path.Combine(tmp, url.Split('/').Last() + ".zip");
        await new WebClient().DownloadFileTaskAsync(new Uri(url), zip);
        ZipFile.ExtractToDirectory(zip, dest);
      }
    }
    public static void RunCmd(string cmd, params string[] args)
    {
      var processInfo = new ProcessStartInfo(cmd, string.Join(" ", args))
      {
        RedirectStandardOutput = false,
      };
      var p = Process.Start(processInfo);
      p.WaitForExit();
    }
  }
}
