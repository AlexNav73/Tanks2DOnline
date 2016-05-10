using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.Handle.Base;
using Tanks2DOnline.Core.Net.TestObjects;

namespace Tanks2DOnline.Client.ConsoleClient.Handles
{
    public class SmallObjectProcessHandle : HandlerBase<SmallTestObject>
    {
        public override void Process(SmallTestObject obj)
        {
            LogManager.Info("SmallTestObject: {0}", obj.Message);
        }
    }
}
