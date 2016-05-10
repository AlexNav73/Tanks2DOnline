using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.Handle.Base;
using Tanks2DOnline.Core.Net.TestObjects;

namespace Tanks2DOnline.Client.ConsoleClient.Handles
{
    public class BigObjectProcessHandle : HandlerBase<BigTestObject>
    {
        public override void Process(BigTestObject obj)
        {
            LogManager.Info("BigData received. {0}", obj.Message);
        }
    }
}
