using System.Threading.Tasks;
using Grpc.Core;

namespace Csi.V0.Server
{
    public sealed class IdentityRpcService : Identity.IdentityBase
    {
        private static readonly GetPluginCapabilitiesResponse pluginCapabilities = new GetPluginCapabilitiesResponse
        {
            Capabilities =
            {
                new PluginCapability
                {
                    Service = new PluginCapability.Types.Service
                    {
                        Type = PluginCapability.Types.Service.Types.Type.ControllerService,
                    },
                }
            }
        };

        private static readonly ProbeResponse probe = new ProbeResponse();

        private readonly GetPluginInfoResponse pluginInfo;

        public IdentityRpcService(string name, string version)
        {
            pluginInfo = new GetPluginInfoResponse
            {
                Name = name,
                VendorVersion = version,
            };
        }

        public override Task<GetPluginInfoResponse> GetPluginInfo(
            GetPluginInfoRequest request,
            ServerCallContext context)
            => Task.FromResult(pluginInfo);

        public override Task<GetPluginCapabilitiesResponse> GetPluginCapabilities(
            GetPluginCapabilitiesRequest request,
            ServerCallContext context)
            => Task.FromResult(pluginCapabilities);

        public override Task<ProbeResponse> Probe(ProbeRequest request, ServerCallContext context)
            => Task.FromResult(probe);
    }
}
