using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net.CommonData;
using Tanks2DOnline.Server.ServerApp.Match;
using Tanks2DOnline.Server.ServerApp.PacketProcessors;

namespace Tanks2DOnline.Server.ServerApp
{
    public class MainProcessor
    {
        private readonly Dictionary<PacketType, IProcessor> _processors 
            = new Dictionary<PacketType, IProcessor>()
        {
            {PacketType.Registration, new RegistrationProcessor()}
        };

        public object Process(ProcessorContext context)
        {
            return _processors[context.PacketType].Process(context);
        }
    }
}
