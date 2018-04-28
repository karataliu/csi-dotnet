using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Logging;

namespace Csi
{
    class FileSocketEndpointHandler : IDisposable
    {
        private const string host = "127.0.0.1";
        private const int port = 10000;
        private const string scBIn = "socat";
        private ILogger logger = GrpcEnvironment.Logger;
        private Process process;

        public ServerPort Handle(string socket)
        {
            if (File.Exists(socket)) File.Delete(socket);

            var processInfo = new ProcessStartInfo(scBIn)
            {
                Arguments = $"-d UNIX-LISTEN:{socket},fork TCP4:{host}:{port}",
                RedirectStandardOutput = false,
            };

            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        process = Process.Start(processInfo);
                        process.WaitForExit();
                        logger.Warning("process exits, will restart.");
                    }
                    catch (Exception ex)
                    {
                        logger.Warning("exception " + ex.Message);
                    }
                    Thread.Sleep(2000);
                }
            });

            return new ServerPort(host, port, ServerCredentials.Insecure);
        }

        #region IDisposable Support
        private bool disposedValue = false;

        public void Dispose()
        {
            if (!disposedValue)
            {
                logger.Info("Stopping process");
                process?.Kill();
                disposedValue = true;
            }
        }
        #endregion

    }
}
