using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net.Packet;

namespace Tanks2DOnline.Server.ConsoleServer.Actions
{
    public interface IAction
    {
        void Process(ServerState serverState, Packet packet);
    }
}
