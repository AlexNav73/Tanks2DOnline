using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net.DataTransfer;
using Tanks2DOnline.Core.Net.Handle;
using Tanks2DOnline.Core.Net.Handle.Interfaces;
using Tanks2DOnline.Core.Net.Packet;
using Tanks2DOnline.Core.Net.TestObjects;
using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Client.ConsoleClient.Handles.ClientActions
{
    public class ConsumingHandler : IPacketHandle
    {
        private readonly HandleStorage _handles;
        private readonly BlockingCollection<Packet> _receivingQueue = new BlockingCollection<Packet>();
        private readonly Dictionary<DataType, Type> _maps = new Dictionary<DataType, Type>()
        {
            {DataType.State, typeof(SmallTestObject)}
        }; 

        public ConsumingHandler(HandleStorage handles)
        {
            _handles = handles;
            Task.Factory.StartNew(ProcessingLoop);
        }

        private void ProcessingLoop()
        {
            foreach (var obj in _receivingQueue.GetConsumingEnumerable())
            {
                var type = obj.DataType;
                var objType = _maps[type];
                _handles[type].Process(DataHelper.ExtractData(objType, obj));
            }
        }

        public void Process(Packet packet)
        {
            _receivingQueue.Add(packet);
        }
    }
}
