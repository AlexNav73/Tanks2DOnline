using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net.DataTransfer;

namespace Tanks2DOnline.Server.ConsoleServer
{
    public class ServerState
    {
        public UserMapCollection Users { get; set; }
        public UdpClient Client { get; set; }
    }
}
