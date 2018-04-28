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
            ICsiRpcServer s1 = new CsiRpcServer(new SampleFactory());
            //s1.SetServiceTypeFromEnvironment();
            s1.ServiceType = CsiRpcServiceType.Identity;
            s1.Start();
        }
    }

    class SampleFactory : ICsiRpcServiceFactory
    {
        public Controller.ControllerBase CreateControllerRpcService()
        {
            throw new NotImplementedException();
        }

        public Identity.IdentityBase CreateIdentityRpcService()
        {
            return new IdentityRpcService("a", "v1");
        }

        public Node.NodeBase CreateNodeRpcService()
        {
            throw new NotImplementedException();
        }
    }
}
