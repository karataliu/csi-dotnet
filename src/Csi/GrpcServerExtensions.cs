using System.Collections.Generic;
using Grpc.Core;

namespace Csi.V0.Server
{
    static class GrpcServerExtensions
    {
        public static void AddServices(this Grpc.Core.Server server,
            IEnumerable<ServerServiceDefinition> ssds)
        {
            foreach (var ssd in ssds) server.Services.Add(ssd);
        }
    }
}
