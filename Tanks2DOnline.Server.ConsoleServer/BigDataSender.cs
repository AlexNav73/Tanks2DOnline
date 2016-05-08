using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Extensions;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.DataTransfer;
using Tanks2DOnline.Core.Net.Packet;
using Tanks2DOnline.Core.Serialization;
using Tanks2DOnline.Server.ConsoleServer.Actions;

namespace Tanks2DOnline.Server.ConsoleServer
{
    public class BigDataSender
    {
        private class UserSendState
        {
            public int Cursor { get; set; }
            public IPEndPoint User { get; set; }
        }
        private class ThreadState
        {
            public List<Packet> Packets { get; set; }
            public List<IPEndPoint> Users { get; set; }
            public UdpClient Client { get; set; }
        }

        private readonly int _threadCount;
        private readonly UdpClient _sender;
        private readonly ConcurrentQueue<IPEndPoint> _userWichRecvedData; 

        public BigDataSender(UdpClient sender, ConcurrentQueue<IPEndPoint> usersWichRecvedData, int threadCount = 5)
        {
            _threadCount = threadCount;
            _sender = sender;
            _userWichRecvedData = usersWichRecvedData;
        }

        public void Send<T>(T obj, List<IPEndPoint> recipients) where T : SerializableObjectBase
        {
            var packets = DataHelper.SplitToPackets(obj, PacketType.BigDataBatch).ToList();
            recipients.Select((v, i) => new { User = v, Index = i % _threadCount })
                .GroupBy(v => v.Index)
                .Select(g => g.Select(v => v.User).ToList())
                .ForEach(v => CreateSendGroup(packets, v));
        }

        private void CreateSendGroup(List<Packet> packets, List<IPEndPoint> users)
        {
            ThreadPool.QueueUserWorkItem(SendWork, new ThreadState()
            {
                Packets = packets, 
                Users = users,
                Client = _sender
            });
        }

        private void SendWork(object s)
        {
            var state = (ThreadState) s;

            var packets = state.Packets;
            var users = state.Users;
            var client = state.Client;

            var cursors = users.Select(u => new UserSendState() {Cursor = 0, User = u}).ToList();
            var cursorsCount = cursors.Count;

            while (cursorsCount > 0)
            {
                IPEndPoint user;
                if (_userWichRecvedData.TryDequeue(out user))
                    cursorsCount += MoveCursor(cursors, user, packets.Count);

                for (int i = 0; i < cursors.Count; i++)
                {
                    var packet = packets[cursors[i].Cursor];
                    client.Send(packet, cursors[i].User);
                    LogManager.Info("Packet with Id: {0} sended", packet.Id);
                }
            }
        }

        private int MoveCursor(List<UserSendState> cursors, IPEndPoint user, int messageLength)
        {
            var userIndex = -1;
            for (int i = 0; i < cursors.Count; i++)
            {
                if (cursors[i].User.Equals(user))
                {
                    userIndex = i;
                    break;
                }
            }
            if (userIndex != -1)
            {
                cursors[userIndex].Cursor += 1;
                var sendCompleted = cursors[userIndex].Cursor == messageLength;
                if (sendCompleted) cursors.RemoveAt(userIndex);

                return sendCompleted ? -1 : 0;
            }
            cursors.Add(new UserSendState() { Cursor = 0, User = user });
            return 1;
        }
    }
}
