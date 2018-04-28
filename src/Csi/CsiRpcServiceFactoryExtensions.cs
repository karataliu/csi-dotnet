using System;
using System.Collections.Generic;
using System.Linq;
using Grpc.Core;

namespace Csi.V0.Server
{
    static class CsiRpcServiceFactoryExtensions
    {
        public static IEnumerable<ServerServiceDefinition> CreateDefinitions(
           this CsiRpcServiceType csiRpcServiceType,
           Func<CsiRpcServiceType, ServerServiceDefinition> create)
            => EnumHelper.AllValues<CsiRpcServiceType>()
                .Where(t => csiRpcServiceType.HasFlag(t))
                .Select(create);

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
