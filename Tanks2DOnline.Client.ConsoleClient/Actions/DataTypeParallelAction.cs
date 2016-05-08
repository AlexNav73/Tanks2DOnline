using System;
using System.Collections.Generic;
using Tanks2DOnline.Core.Net.Action.Base;
using Tanks2DOnline.Core.Net.DataTransfer;
using Tanks2DOnline.Core.Net.Handle.Base;
using Tanks2DOnline.Core.Net.Packet;
using Tanks2DOnline.Core.Net.TestObjects;
using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Client.ConsoleClient.Actions
{
    public class DataTypeParallelAction : ParallelPacketAction
    {
        private readonly Dictionary<DataType, Type> _maps = new Dictionary<DataType, Type>()
        {
            {DataType.State, typeof(SmallTestObject)}
        }; 

        protected override bool IsSupported(Packet packet)
        {
            return packet.Type == PacketType.State;
        }

        protected override void HandleAsync(Packet packet)
        {
            var type = packet.DataType;
            if (Handles[type] != null)
                Handles[type].Process(DataHelper.ExtractData(_maps[type], packet));
        }
    }
}
