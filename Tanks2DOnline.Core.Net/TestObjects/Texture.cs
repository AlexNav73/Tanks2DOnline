using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Serialization;
using Tanks2DOnline.Core.Serialization.Attributes;

namespace Tanks2DOnline.Core.Net.TestObjects
{
    public class Texture : SerializableObjectBase
    {
        [Mark] public byte[] Data { get; set; }

        public override DataType GetDataType()
        {
            return DataType.Texture;
        }

        public Texture() { }

        public Texture(string path)
        {
            Data = File.ReadAllBytes(path);
        }

        protected override void AfterDeserialization()
        {
            var rand = new Random();
            var fileName = Path.Combine("Textures", rand.Next()%10000 + ".jpg");
            File.WriteAllBytes(fileName, Data);
        }
    }
}
