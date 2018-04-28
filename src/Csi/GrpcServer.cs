﻿using System.Collections.Generic;
using Grpc.Core;

namespace Csi.Internal
{
    sealed class GrpcServer
    {
        private readonly Server server = new Server();

        public void Start() => server.Start();

        public void AddServices(IEnumerable<ServerServiceDefinition> ssds)
        {
            foreach (var ssd in ssds) server.Services.Add(ssd);
        }

        public void AddEndpoint(ServerPort serverPort)
            => server.Ports.Add(serverPort);
    }
}