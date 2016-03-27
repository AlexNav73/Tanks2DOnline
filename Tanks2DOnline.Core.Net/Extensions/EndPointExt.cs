﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Tanks2DOnline.Core.Net.Extensions
{
    public static class EndPointExt
    {
        public static EndPoint Parse(string address)
        {
            var parts = address.Split(new[] {':'}, StringSplitOptions.RemoveEmptyEntries);
            return new IPEndPoint(IPAddress.Parse(parts[0]), int.Parse(parts[1]));
        }
    }
}
