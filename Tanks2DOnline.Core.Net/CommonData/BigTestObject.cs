using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net.Serialization;
using Tanks2DOnline.Core.Net.Serialization.Attributes;

namespace Tanks2DOnline.Core.Net.CommonData
{
    public class BigTestObject : SerializableObjectBase
    {
        [Mark] public byte[] Bytes { get; set; }
        public string Message { get; set; }

        public BigTestObject()
        {
            Bytes = new byte[1000000];

            int j = 0;

            for (int i = 0; i < 1000000; i++)
            {
                if (i % 25535 == 0) { j++; }
                Bytes[i] = (byte) (j);
            }

            Message = "Recived";
        }

    }
}
