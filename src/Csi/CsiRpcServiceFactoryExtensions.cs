using Grpc.Core;
using System;

namespace Csi.V0.Server
{
    static class CsiRpcServiceFactoryExtensions
    {
        public static ServerServiceDefinition CreateDefinition(
            this ICsiRpcServiceFactory csiRpcServiceFactory,
            CsiRpcServiceType csiRpcServiceTypeSingle)
        {
            switch (csiRpcServiceTypeSingle)
            {
                case CsiRpcServiceType.Identity:
                    return Identity.BindService(csiRpcServiceFactory.CreateIdentityRpcService());
                case CsiRpcServiceType.Controller:
                    return Controller.BindService(csiRpcServiceFactory.CreateControllerRpcService());
                case CsiRpcServiceType.Node:
                    return Node.BindService(csiRpcServiceFactory.CreateNodeRpcService());
            }

            throw new Exception("Unsupported service type " + csiRpcServiceTypeSingle);
        }
    }
}
