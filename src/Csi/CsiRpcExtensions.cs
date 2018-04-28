using System;
using System.Linq;

namespace Csi.V0.Server
{
    public static class CsiRpcExtensions
    {
        public static void SetServiceTypeFromEnvironment(
            this CsiRpcServer csiRpcServer,
            string disablePrefix = "CSI_SERVICE_DISABLE_")
        {
            var stype = CsiRpcServiceType.None;
            foreach (var st in Enum.GetValues(typeof(CsiRpcServiceType)).Cast<CsiRpcServiceType>())
            {
                var disableVar = disablePrefix + st.ToString().ToUpper();
                if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(disableVar))) stype |= st;
            }
            csiRpcServer.ServiceType = stype;
        }

        public static void SetEndpointFromEnvironment(
          this CsiRpcServer csiRpcServer,
          string epVar = "CSI_ENDPOINT")
        {
            var ep = Environment.GetEnvironmentVariable(epVar);
            if (ep != null) csiRpcServer.Endpoint = ep;
        }

        public static void ConfigFromEnvironment(this CsiRpcServer csiRpcServer)
        {
            csiRpcServer.SetServiceTypeFromEnvironment();
            csiRpcServer.SetEndpointFromEnvironment();
        }
    }
}
