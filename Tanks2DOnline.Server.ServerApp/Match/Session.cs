using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net.CommonData;

namespace Tanks2DOnline.Server.ServerApp.Match
{
    public class Session
    {
        private readonly Dictionary<EndPoint, ClientInfo> _players;
        private const int PlayersCount = 6;

        public Session()
        {
            _players = new Dictionary<EndPoint, ClientInfo>();
        }

        public bool Add(EndPoint point, ClientInfo player)
        {
            if (_players.Count < PlayersCount)
            {
                _players.Add(point, player);
                return true;
            }

            return false;
        }
    }
}
