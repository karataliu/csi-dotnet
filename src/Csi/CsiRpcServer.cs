using System.Collections.Generic;
using System.Runtime.InteropServices;
using Csi.Internal;
using Grpc.Core;
using Grpc.Core.Logging;

namespace Csi.V0.Server
{
    public sealed class CsiRpcServer : ICsiRpcServer
    {
        public string Endpoint { get; set; } = "127.0.0.1:10000";
        public CsiRpcServiceType ServiceType { get; set; }

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
            var grpcServer = new GrpcServer();
            grpcServer.AddEndpoint(generateEndpoint());
            GrpcEnvironment.Logger.Info("Listening at: {0}", Endpoint);
            grpcServer.AddServices(generateDefinitions());

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

            if(fse!=null && Endpoint.StartsWith("/"))
            {
                return fse.Handle(Endpoint);
            }

            throw new System.Exception("Unsupported endpoint " + Endpoint);
        }

        private IEnumerable<ServerServiceDefinition> generateDefinitions()
        {
            ILogger logger = GrpcEnvironment.Logger;

            if (ServiceType.HasFlag(CsiRpcServiceType.Identity))
            {
                logger.Info("Adding {0} service", CsiRpcServiceType.Identity);
                yield return Identity.BindService(csiRpcServiceFactory.CreateIdentityRpcService());
            }
            if (ServiceType.HasFlag(CsiRpcServiceType.Controller))
            {
                logger.Info("Adding {0} service", CsiRpcServiceType.Controller);
                yield return Controller.BindService(csiRpcServiceFactory.CreateControllerRpcService());
            }
            if (ServiceType.HasFlag(CsiRpcServiceType.Node))
            {
                logger.Info("Adding {0} service", CsiRpcServiceType.Node);
                yield return Node.BindService(csiRpcServiceFactory.CreateNodeRpcService());
            }
        }
    }
}
