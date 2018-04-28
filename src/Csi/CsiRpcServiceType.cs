using System;

namespace Csi.V0.Server
{
    [Flags]
    public enum CsiRpcServiceType
    {
        None = 0,
        Identity = 1,
        Controller = 2,
        Node = 4,
    }
}
