using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.Handle.Base;
using Tanks2DOnline.Core.Net.TestObjects;

namespace Tanks2DOnline.Client.ConsoleClient.Handles
{
    public class BigObjectProcessHandle : OnReceiveHandler<BigTestObject>
    {
        public override void Process(BigTestObject obj)
        {
            LogManager.Info("BigData received. {0}", obj.Message);
        }
    }
}
