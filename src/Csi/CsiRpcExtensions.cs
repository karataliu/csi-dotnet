using System;
using System.Linq;

namespace Csi.V0.Server
{
    public static class CsiRpcExtensions
    {
        public static void SetServiceTypeFromEnvironment(
            this ICsiRpcServer csiRpcServer,
            string disablePrefix = "CSI_SERVICE_DISABLE_")
        {
            var stype = CsiRpcServiceType.None;
            foreach (var st in Enum.GetValues(typeof(CsiRpcServiceType)).Cast<CsiRpcServiceType>())
            {
                var disableVar = disablePrefix + st.ToString().ToUpper();
                if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(disableVar))) stype |= st;
            }
            Console.WriteLine(stype);
            csiRpcServer.ServiceType = stype;
        }
    }
}
