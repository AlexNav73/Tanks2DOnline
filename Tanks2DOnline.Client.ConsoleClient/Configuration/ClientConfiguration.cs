using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks2DOnline.Client.ConsoleClient.Configuration
{
    public class ClientConfiguration
    {
        public string ServerIP { get; set; }
        public int ServerPort { get; set; }
        public int RegistrationTimeout { get; set; }
        public int Port { get; set; }
    }
}
