using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Client.ConsoleClient.Handles.Base;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.TestObjects;

namespace Tanks2DOnline.Client.ConsoleClient.Handles.Implementations
{
    public class SmallObjectProcessHandle : OnReceivedHandle<SmallTestObject>
    {
        protected override void Process(SmallTestObject obj)
        {
            LogManager.Info("SmallTestObject: {0}", obj.Message);
        }

        protected override bool CanProcess(object obj)
        {
            return obj is SmallTestObject;
        }
    }
}
