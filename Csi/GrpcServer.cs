using System;
using Grpc.Core;

namespace Csi
{
    sealed class GrpcServer
    {
        private readonly Server server;

        public GrpcServer() => this.server = new Server();

        public void Start() => server.Start();

        public void AddService<T>(Func<T, ServerServiceDefinition> bind, Func<T> create)
        {
            server.Services.Add(bind(create()));
        }

        public void AddEndpoint(string host, int port)
            => server.Ports.Add(new ServerPort(host, port, ServerCredentials.Insecure));

        private static string getServiceName<T>()
        {
            var fullName = typeof(T).FullName;
            var index = fullName.IndexOf('+');
            if (index > 0) return fullName.Substring(0, index);
            return fullName;
        }
    }
}
