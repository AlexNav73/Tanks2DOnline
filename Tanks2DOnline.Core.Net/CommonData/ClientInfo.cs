using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Serialization;
using Tanks2DOnline.Core.Serialization.Attributes;

namespace Tanks2DOnline.Core.Net.CommonData
{
    public class ClientInfo : SerializableObjectBase
    {
        [Mark] public string Name { get; set; }
        [Mark] public byte[] Data { get; set; }

        public override DataSize GetSize()
        {
            return DataSize.Small;
        }
    }
}
