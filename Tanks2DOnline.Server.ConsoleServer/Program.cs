using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net;
using Tanks2DOnline.Core.Net.CommonData;

namespace Tanks2DOnline.Server.ConsoleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (UdpSocket socket = new UdpSocket())
            {
                socket.Bind(IPAddress.Any);

                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 4242);
                EndPoint rmEndPoint = (EndPoint)remoteEndPoint;

                var packet = socket.RecvPacket(ref rmEndPoint);

                Console.WriteLine(packet.Client.Name);
            }
        }
    }
}
