using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Collections.Base;

namespace Tanks2DOnline.Server.ConsoleServer
{
    public class UserMapCollection : FastSearchCollection<string, IPEndPoint>
    {
        protected override int Cmp(string lhs, string rhs)
        {
            return lhs.CompareTo(rhs);
        }

        protected override int Cmp(IPEndPoint lhs, IPEndPoint rhs)
        {
            return lhs.Port.CompareTo(rhs.Port);
        }

        public List<IPEndPoint> GetAllExcept(IPEndPoint remote)
        {
            var result = new List<IPEndPoint>();
            GetAllExcept(result, (LinkedObject)Root.Left, remote);
            return result;
        }

        private void GetAllExcept(List<IPEndPoint> result, LinkedObject root, IPEndPoint remote)
        {
            if (root != null)
            {
                if (!root.Equals(remote)) result.Add(root.Link);
                GetAllExcept(result, (LinkedObject)root.Left, remote);
                GetAllExcept(result, (LinkedObject)root.Right, remote);
            }
        }
    }
}
