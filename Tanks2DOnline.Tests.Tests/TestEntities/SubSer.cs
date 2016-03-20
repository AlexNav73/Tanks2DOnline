using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net.CommonData;
using Tanks2DOnline.Core.Net.Serialization;
using Tanks2DOnline.Core.Net.Serialization.Attributes;

namespace Tanks2DOnline.Tests.Tests.TestEntities
{
    public class SubSer : SerializableObjectBase
    {
        [Mark] public string PropString { get; set; }

        [Mark] public double Double { get; set; }
        [Mark] private float _float;

        [Mark] public PacketType Type { get; set; }

        public SubSer()
        {
            Type = PacketType.Registration;
        }

        public void Init()
        {
            Double = 0.0001;
            _float = 0.012f;

            PropString = "Fuck you again";

            Type = PacketType.HoldsData;
        }
    }
}
