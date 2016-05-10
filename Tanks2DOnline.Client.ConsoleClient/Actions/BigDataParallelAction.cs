using System;
using System.Collections.Generic;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.Action.Base;
using Tanks2DOnline.Core.Net.Helpers;
using Tanks2DOnline.Core.Net.Packet;
using Tanks2DOnline.Core.Net.TestObjects;
using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Client.ConsoleClient.Actions
{
    public class BigDataParallelAction : ParallelActionBase
    {
        private readonly List<Packet> _buffer = new List<Packet>(); 
        private readonly HashSet<int> _uniqIds = new HashSet<int>(); 
        private readonly Dictionary<DataType, Type> _map = new Dictionary<DataType, Type>()
        {
            {DataType.BigData, typeof(BigTestObject)}
        }; 

        protected override bool IsSupported(Packet packet)
        {
            return packet.Type == PacketType.BigDataBatch;
        }

        protected override void HandleAsync(Packet packet)
        {
//            LogManager.Debug("BigData packet received. Id: {0}", packet.Id);
            State.Client.Send(PacketFactory.TypedPacket(PacketType.PacketAcceptResponse), packet.Address);

            if (_uniqIds.Contains(packet.Id + 1)) return; // + 1 needs, because clean HasSet contains default values of int
                                                          // and packet id starts from 0. This means, empty HashSet always
                                                          // contains packet id 0, this is not good.

            _buffer.Add(packet);
            _uniqIds.Add(packet.Id + 1);

            if (_buffer.Count == packet.Count)
            {
                Handles[packet.DataType].Process(DataHelper.ExtractData(_map[packet.DataType], _buffer));
                _buffer.Clear();
                _uniqIds.Clear();
            }
        }
    }
}
