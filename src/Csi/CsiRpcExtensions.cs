using System;

namespace Csi.V0.Server
{
    public static class CsiRpcExtensions
    {
        public static void ConfigServiceTypeFromEnvironment(
            this ICsiRpcServer csiRpcServer,
            string disablePrefix = "CSI_SERVICE_DISABLE_")
        {
            var stype = default(CsiRpcServiceType);
            foreach (var st in EnumHelper.AllValues<CsiRpcServiceType>())
            {
                var disableVar = disablePrefix + st.ToString().ToUpper();
                if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(disableVar))) stype |= st;
            }
            csiRpcServer.ServiceType = stype;
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
