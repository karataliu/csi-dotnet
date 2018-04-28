using System;
using Csi.Internal;

namespace Csi.V0.Server
{
    public abstract class CsiRpcServer : ICsiRpcServer
    {
        public abstract Identity.IdentityBase CreateIdentityRpcService();
        public abstract Controller.ControllerBase CreateControllerRpcService();
        public abstract Node.NodeBase CreateNodeRpcService();

        public void Start(string host, int port)
        {
            var grpcServer = new GrpcServer();
            if (enableService(nameof(Identity)))
                grpcServer.AddService(Identity.BindService, CreateIdentityRpcService);
            if (enableService(nameof(Controller)))
                grpcServer.AddService(Controller.BindService, CreateControllerRpcService);
            if (enableService(nameof(Node)))
                grpcServer.AddService(Node.BindService, CreateNodeRpcService);
            grpcServer.AddEndpoint(host, port);

            grpcServer.Start();
        }

        private bool enableService(string serviceName)
        {
            var disableVar = "CSI_SERVICE_DISABLE_" + serviceName.ToUpper();
            return string.IsNullOrEmpty(Environment.GetEnvironmentVariable(disableVar));
        }
    }
}
