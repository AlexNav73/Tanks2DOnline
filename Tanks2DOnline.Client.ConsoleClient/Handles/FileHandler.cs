using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.Handle.Base;
using Tanks2DOnline.Core.Net.Handle.Interfaces;
using Tanks2DOnline.Core.Net.TestObjects;

namespace Tanks2DOnline.Client.ConsoleClient.Handles
{
    public class FileHandler : HandlerBase<Texture>
    {
        public override void Process(Texture obj)
        {
            LogManager.Info("Texture received. {0} Bytes", obj.Data.Length);
        }
    }
}
