using System;
using Csi.V0;
using Csi.V0.Server;
using Grpc.Core;
using Grpc.Core.Logging;

namespace Csi.SampleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            GrpcEnvironment.SetLogger(new ConsoleLogger());
            ICsiRpcServer s1 = new Server1();
            s1.SetServiceTypeFromEnvironment();
            s1.Start();
        }
    }

    class Server1 : CsiRpcServer
    {
       
        public override Controller.ControllerBase CreateControllerRpcService()
        {
            throw new NotImplementedException();
        }

        public override Identity.IdentityBase CreateIdentityRpcService()
        {
            return new IdentityRpcService("a", "v1");
        }

        public override Node.NodeBase CreateNodeRpcService()
        {
            throw new NotImplementedException();
        }
    }
}
