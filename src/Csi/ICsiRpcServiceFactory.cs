namespace Csi.V0.Server
{
    public interface ICsiRpcServiceFactory
    {
        Identity.IdentityBase CreateIdentityRpcService();
        Controller.ControllerBase CreateControllerRpcService();
        Node.NodeBase CreateNodeRpcService();
    }
}
