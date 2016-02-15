using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net.CommonData;

namespace Tanks2DOnline.Server.ServerApp.Match
{
    public class MatchMaker
    {
        private readonly List<Session> _sessions;

        public MatchMaker()
        {
            _sessions = new List<Session>();
        }

        public void RegisterPlayer(EndPoint point, ClientInfo player)
        {
            if (_sessions.Count == 0)
            {
                _sessions.Add(new Session());
                _sessions[0].Add(point, player);
            }
            else if (!_sessions.Any(s => s.Add(point, player)))
            {
                var session = new Session();
                session.Add(point, player);
                _sessions.Add(session);
            }
        }
    }
}
