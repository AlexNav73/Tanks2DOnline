using System.Security.Cryptography.X509Certificates;

namespace Tanks2DOnline.Core.Net.DataTransfer
{
    public interface IUdpClientState
    {
        UdpClient Client { get; set; }
    }
}