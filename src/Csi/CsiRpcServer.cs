using Csi.Internal;
using Grpc.Core;
using Grpc.Core.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Csi.V0.Server
{
    public interface ICsiRpcServer
    {
        string Endpoint { set; }
        CsiRpcServiceType ServiceType { set; }
        void Start();
    }

    public sealed class CsiRpcServer : ICsiRpcServer
    {
        public string Endpoint { get; set; } = "127.0.0.1:10000";
        public CsiRpcServiceType ServiceType { get; set; }

        private readonly ICsiRpcServiceFactory csiRpcServiceFactory;
        private readonly ILogger logger = GrpcEnvironment.Logger;

        public CsiRpcServer(ICsiRpcServiceFactory csiRpcServiceFactory)
            => this.csiRpcServiceFactory = csiRpcServiceFactory;

        public void Start()
        {
            new GrpcServer(Endpoint, createDefinitions()).Start();
            logger.Info("Listen at: {0}", Endpoint);
        }

        public IEnumerable<ServerServiceDefinition> createDefinitions()
        {
            if (ServiceType == default(CsiRpcServiceType))
            {
                logger.Warning("No service loaded, set ServiceType property to enable service");
                return Enumerable.Empty<ServerServiceDefinition>();
            }

            return EnumHelper.AllValues<CsiRpcServiceType>()
               .Where(t => ServiceType.HasFlag(t))
               .Select(t =>
               {
                   logger.Info("Load {0} service", t);
                   return csiRpcServiceFactory.CreateDefinition(t);
               });
        }
    }
}
