using Csi.V0.Server;
using Xunit;

namespace Csi.Test
{
    public class CsiRpcServiceTypeTest
    {
        [Theory]
        [InlineData(CsiRpcServiceType.Identity | CsiRpcServiceType.Controller | CsiRpcServiceType.Node, "")]
        [InlineData(CsiRpcServiceType.Identity | CsiRpcServiceType.Controller | CsiRpcServiceType.Node, null)]
        [InlineData(CsiRpcServiceType.Identity | CsiRpcServiceType.Controller, "node")]
        [InlineData(CsiRpcServiceType.Identity | CsiRpcServiceType.Controller, "nOde")]
        [InlineData(default(CsiRpcServiceType), "NODE,IDENTITY,CONTROLLER")]
        public void ParseDisables(CsiRpcServiceType expected, string disables) 
            => Assert.Equal(expected, CsiRpcServiceTypeHelper.ParseDisables(disables));
    }
}
