using System;

namespace Csi.V0.Server
{
    public static class CsiRpcServerExtensions
    {
        public static void ConfigServiceTypeFromEnvironment(
            this ICsiRpcServer csiRpcServer,
            string disableVar = "CSIEXT_SERVICE_DISABLES")
        {
            csiRpcServer.ServiceType 
                = CsiRpcServiceTypeHelper.ParseDisables(Environment.GetEnvironmentVariable(disableVar));
        }

        public static void ConfigEndpointFromEnvironment(
          this ICsiRpcServer csiRpcServer,
          string epVar = "CSI_ENDPOINT")
        {
            var ep = Environment.GetEnvironmentVariable(epVar);
            if (ep != null) csiRpcServer.Endpoint = ep;
        }

        public static void ConfigFromEnvironment(this ICsiRpcServer csiRpcServer)
        {
            csiRpcServer.ConfigServiceTypeFromEnvironment();
            csiRpcServer.ConfigEndpointFromEnvironment();
        }
    }
}
