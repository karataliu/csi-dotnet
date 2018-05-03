using Csi.Internal;
using FluentAssertions;
using Xunit;

namespace Csi.Test
{
    public class GrpcEndpointTest
    {
        [Theory]
        [InlineData("127.0.0.1", 1000, "ipv4://127.0.0.1:1000")]
        [InlineData("127.0.0.3", 1300, "ipv4://127.0.0.3:1300")]
        [InlineData("127.0.0.1", 1000, "127.0.0.1:1000")]
        public void Ipv4(string host, int port, string endpoint)
        {
            var e1 = GrpcEndpoint.Parse(endpoint).Should().BeOfType<GrpcEndpoint.Ipv4>().Subject;
            e1.Host.Should().Be(host);
            e1.Port.Should().Be(port);
        }

        [Theory]
        [InlineData("/tmp/1", "unix:///tmp/1")]
        [InlineData("/tmp/1", "/tmp/1")]
        public void Socket(string path, string endpoint)
        {
            GrpcEndpoint.Parse(endpoint).Should().BeOfType<GrpcEndpoint.Unix>()
                .Subject.Path.Should().Be(path);
        }
    }
}
