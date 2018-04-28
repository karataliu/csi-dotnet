using System.Runtime.InteropServices;
using Grpc.Core;
using Grpc.Core.Logging;

namespace Csi.V0.Server
{
    public sealed class CsiRpcServer
    {
        public string Endpoint { get; set; } = "127.0.0.1:10000";
        public CsiRpcServiceType ServiceType { get; set; }

        private readonly ILogger logger = GrpcEnvironment.Logger;
        private readonly ICsiRpcServiceFactory csiRpcServiceFactory;
        private readonly FileSocketEndpointHandler fse;

        public CsiRpcServer(ICsiRpcServiceFactory csiRpcServiceFactory)
        {
            this.csiRpcServiceFactory = csiRpcServiceFactory;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                fse = new FileSocketEndpointHandler();
        }

        public void Start()
        {
            var grpcServer = new Grpc.Core.Server();
            var definitions = ServiceType.CreateDefinitions(t =>
            {
                logger.Info("Load {0} service", t);
                return csiRpcServiceFactory.CreateDefinition(t);
            });
            grpcServer.AddServices(definitions);
            grpcServer.Ports.Add(generateEndpoint());
            logger.Info("Listening at: {0}", Endpoint);
            grpcServer.Start();
        }

        private ServerPort generateEndpoint()
        {
            var idx = Endpoint.IndexOf(":");
            if (idx > 0)
            {
                return new ServerPort(
                    Endpoint.Substring(0, idx),
                    int.Parse(Endpoint.Substring(idx + 1)),
                    ServerCredentials.Insecure);
            }

            if (fse != null && Endpoint.StartsWith("/"))
            {
                return fse.Handle(Endpoint);
            }

            throw new System.Exception("Unsupported endpoint " + Endpoint);
        }
    }
}
