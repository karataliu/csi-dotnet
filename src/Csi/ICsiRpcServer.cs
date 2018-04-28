using System;

namespace Csi.V0.Server
{
    public interface ICsiRpcServer
    {
        string Endpoint { set; }
        CsiRpcServiceType ServiceType { set; }
        void Start();
    }

    [Flags]
    public enum CsiRpcServiceType
    {
        None = 0,
        Identity = 1,
        Controller = 2,
        Node = 4,
    }
}
