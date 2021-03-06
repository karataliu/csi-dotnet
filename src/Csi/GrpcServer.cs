﻿using Grpc.Core;
using Grpc.Core.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Csi.Internal
{
    sealed class GrpcServer
    {
        private readonly Server grpcServer = new Server();
        private readonly UnixDomainSocketEndpointHandler fse;

        public GrpcServer(string endpoint, IEnumerable<ServerServiceDefinition> definitions)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                fse = new UnixDomainSocketEndpointHandler();

            grpcServer.Ports.Add(generateEndpoint(endpoint));
            foreach (var ssd in definitions) grpcServer.Services.Add(ssd);
        }

        public void Start() => grpcServer.Start();

        private ServerPort generateEndpoint(string endpoint)
        {
            switch (GrpcEndpoint.Parse(endpoint)){
                case GrpcEndpoint.Ipv4 ipv4:
                    return new ServerPort(
                        ipv4.Host,
                        ipv4.Port,
                        ServerCredentials.Insecure);
                case GrpcEndpoint.Unix unix:
                    if (fse != null) return fse.Handle(unix.Path);
                    break;
            }

            throw new Exception("Unsupported endpoint " + endpoint);
        }
    }

    class UnixDomainSocketEndpointHandler : IDisposable
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
