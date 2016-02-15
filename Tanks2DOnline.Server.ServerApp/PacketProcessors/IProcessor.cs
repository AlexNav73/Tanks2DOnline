using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks2DOnline.Server.ServerApp.PacketProcessors
{
    public interface IProcessor
    {
        object Process(ProcessorContext context);
    }
}
