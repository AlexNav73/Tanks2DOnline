using Tanks2DOnline.Core.Serialization;
using Tanks2DOnline.Core.Serialization.Attributes;

namespace Tanks2DOnline.Core.Net.TestObjects
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
