using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net.DataTransfer;

namespace Tanks2DOnline.Client.ConsoleClient
{
    public class ClientState : IUdpClientState
    {
        public UdpClient Client { get; set; }
    }
}
