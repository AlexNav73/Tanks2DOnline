using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.CommonData;
using Tanks2DOnline.Core.Net.DataTransfer.Base;
using Tanks2DOnline.Core.Net.DataTransfer.Scenario;
using Tanks2DOnline.Core.Serialization;
using Tanks2DOnline.Core.Serialization.Attributes;

namespace Tanks2DOnline.Core.Net.DataTransfer
{
    public class DataTransferManager : IDisposable
    {
        private bool _isDisposed = false;
        public readonly int Port = 4242;

        private readonly Dictionary<DataSize, PacketTransferBase> _protocols
            = new Dictionary<DataSize, PacketTransferBase>();

        public DataTransferManager(IPAddress selfIp, IPAddress remoteIp)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            if (selfIp != null) 
                socket.Bind(new IPEndPoint(selfIp, Port));

            var dgramm = new UdpDatagrams(socket);
            dgramm.SetRemote(remoteIp);

            var stream = new UdpStream(socket);
            stream.SetRemote(remoteIp);

            _protocols.Add(DataSize.Small, dgramm);
            _protocols.Add(DataSize.Big, stream);
        }

        public void SendData<T>(T data, PacketType type) where T : SerializableObjectBase
        {
            try
            {
                var attr = typeof(T).GetCustomAttribute<SizableAttribute>();
                var size = attr != null ? attr.Size : DataSize.Small;

                if (_protocols.ContainsKey(size))
                {
                    _protocols[size].Send(data, type);
                }
            }
            catch (Exception e)
            {
                LogManager.Error("DataTransferManager Send: {0}", e.Message);
                throw;
            }
        }

        public T RecvData<T>() where T : SerializableObjectBase
        {
            try
            {
                var attr = typeof(T).GetCustomAttribute<SizableAttribute>();
                var size = attr != null ? attr.Size : DataSize.Small;

                if (_protocols.ContainsKey(size))
                {
                    return _protocols[size].Recv<T>();
                }
                return null;
            }
            catch (Exception e)
            {
                LogManager.Error("DataTransferManager Recv: {0}", e.Message);
                throw;
            }
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                _protocols.First().Value.Dispose();
            }
        }
    }
}
