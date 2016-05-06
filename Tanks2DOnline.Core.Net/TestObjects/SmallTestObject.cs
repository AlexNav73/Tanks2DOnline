using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Serialization;
using Tanks2DOnline.Core.Serialization.Attributes;

namespace Tanks2DOnline.Core.Net.TestObjects
{
    public class SmallTestObject : SerializableObjectBase
    {
        [Mark] public int Id { get; set; }
        [Mark] public string Message { get; set; }
        [Mark] public double Double { get; set; }

        public static SmallTestObject Create()
        {
            return new SmallTestObject()
            {
                Id = 6,
                Double = 13.5,
                Message = "Hi, bitch!"
            };
        }

        public override DataType GetDataType()
        {
            return DataType.State;
        }
    }
}
