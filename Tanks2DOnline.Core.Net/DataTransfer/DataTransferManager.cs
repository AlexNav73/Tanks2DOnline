using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.DataTransfer.Base;
using Tanks2DOnline.Core.Net.DataTransfer.Interfaces;
using Tanks2DOnline.Core.Net.DataTransfer.Scenario;
using Tanks2DOnline.Core.Net.Packet;
using Tanks2DOnline.Core.Serialization;
using Tanks2DOnline.Core.Serialization.Attributes;

namespace Tanks2DOnline.Core.Net.DataTransfer
{
    public class DataTransferManager : IDisposable
    {
        private bool _isDisposed = false;

        private readonly Dictionary<DataSize, IDataTransferer> _protocols
            = new Dictionary<DataSize, IDataTransferer>();

        public DataTransferManager(IPAddress selfIp, int port)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            if (selfIp != null) 
                socket.Bind(new IPEndPoint(selfIp, port));

            _protocols.Add(DataSize.Packet, new BlockingDatagrams(socket));
            _protocols.Add(DataSize.Small, new UdpDatagrams(socket));
            _protocols.Add(DataSize.Big, new UdpStream(socket));
        }

        public void SendData<T>(EndPoint remote, T data, PacketType type) where T : SerializableObjectBase
        {
            var attr = typeof(T).GetCustomAttribute<SizableAttribute>();
            var size = attr != null ? attr.Size : DataSize.Small;

            if (_protocols.ContainsKey(size))
            {
                _protocols[size].Send(remote, data, type);
            }
        }

        public void RecvData(Type objType, ref EndPoint remote, Action<object> callback)
        {
            var attr = objType.GetCustomAttribute<SizableAttribute>();
            var size = attr != null ? attr.Size : DataSize.Small;

            if (_protocols.ContainsKey(size))
            {
                _protocols[size].Recv(objType, ref remote, callback);
            }
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                ((SimplePasketTransfer)_protocols.First().Value).Dispose();
            }
        }
    }
}
