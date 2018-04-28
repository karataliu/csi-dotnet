using System;
using System.Threading;
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
            var s1 = new CsiRpcServer(new SampleFactory());
            //s1.SetServiceTypeFromEnvironment();
            s1.ServiceType = CsiRpcServiceType.Identity;
            //s1.Endpoint = "127.0.0.18666";
            s1.SetEndpointFromEnvironment();
            s1.Start();

            Thread.Sleep(Timeout.Infinite);
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
