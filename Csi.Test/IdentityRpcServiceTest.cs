using System;
using System.Linq;
using System.Threading.Tasks;
using Csi.V0;
using Csi.V0.Server;
using Grpc.Core;
using Xunit;

namespace Csi.Test
{
    public class IdentityRpcServiceTest
    {
        private ServerCallContext emptyContext = null;

        [Fact]
        public async Task TestGetPluginInfo()
        {
            var name = Guid.NewGuid().ToString();
            var version = "0.1.0";
            var service = new IdentityRpcService(name, version);
            var response = await service.GetPluginInfo(new GetPluginInfoRequest(), emptyContext);
            Assert.Equal(name, response.Name);
            Assert.Equal(version, response.VendorVersion);
        }

        [Fact]
        public async Task TestGetPluginCapabilities()
        {
            var service = new IdentityRpcService("", "");
            var response = await service.GetPluginCapabilities(new GetPluginCapabilitiesRequest(), emptyContext);
            Assert.Single(response.Capabilities);
            Assert.Equal(PluginCapability.Types.Service.Types.Type.ControllerService,
                response.Capabilities.Single().Service.Type);
        }

        [Fact]
        public async Task TestProbe()
        {
            var service = new IdentityRpcService("", "");
            await service.Probe(new ProbeRequest(), emptyContext);
        }
    }
}
