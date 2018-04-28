using System.Collections.Generic;
using Csi.Internal;
using Grpc.Core;
using Grpc.Core.Logging;

namespace Csi.V0.Server
{
    public sealed class CsiRpcServer : ICsiRpcServer
    {
        public string Endpoint { get; set; }
        public CsiRpcServiceType ServiceType { get; set; }

        private readonly ICsiRpcServiceFactory csiRpcServiceFactory;

        public CsiRpcServer(ICsiRpcServiceFactory csiRpcServiceFactory)
        {
            this.csiRpcServiceFactory = csiRpcServiceFactory;
        }

        public void Start()
        {
            var grpcServer = new GrpcServer();
            grpcServer.AddEndpoint("127.0.0.1", 8080);
            grpcServer.AddServices(generateDefinitions());

            grpcServer.Start();
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
