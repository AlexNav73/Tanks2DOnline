using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net.CommonData;

namespace Tanks2DOnline.Server.ServerApp.PacketProcessors
{
    public class RegistrationProcessor : IProcessor
    {
        public object Process(ProcessorContext context)
        {
            context.MatchMaker.RegisterPlayer(context.Point, context.Info);
            return null;
        }
    }
}
