using System;

namespace Csi.Internal
{
    abstract class GrpcEndpoint
    {
        public class Ipv4 : GrpcEndpoint
        {
            public string Host { get; set; }
            public int Port { get; set; }
        }

        public class Unix : GrpcEndpoint
        {
            public string Path { get; set; }
        }

        public static GrpcEndpoint Parse(string endpoint)
        {
            var ep = endpoint;
            if (!ep.Contains("://"))
            {
                if (ep.Contains(":")) ep = "ipv4://" + ep;
                else ep = "unix://" + ep;
            }

            var uri = new Uri(ep);

            switch (uri.Scheme)
            {
                case "ipv4":
                    return new Ipv4
                    {
                        Host = uri.Host,
                        Port = uri.Port,
                    };
                case "unix":
                    return new Unix
                    {
                        Path = uri.AbsolutePath,
                    };
            }

            return null;
        }
    }
}
