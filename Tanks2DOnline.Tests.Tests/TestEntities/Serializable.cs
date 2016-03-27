using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

using Tanks2DOnline.Core.Serialization;
using Tanks2DOnline.Core.Serialization.Attributes;

namespace Tanks2DOnline.Tests.Tests.TestEntities
{
    public class Serializable : SerializableObjectBase
    {
        [Mark] public int PropInt { get; set; }
        [Mark] public string PropString { get; set; }
        [Mark] public bool PropBool { get; set; }

        [Mark] public int Fint;
        [Mark] private int _fint;

        [Mark] public SubSer Inner = new SubSer();

        public void Init()
        {
            PropInt = 42;
            PropString = "Fuck you!";
            PropBool = true;

            Fint = 103;
            _fint = 52;

            Inner.Init();
        }

        public void SerializeToFile(string path)
        {
            using (FileStream file = File.Open(path, FileMode.Create))
            {
                byte[] buf = Serialize();
                file.Write(buf, 0, buf.Length);
            }
        }

        public Serializable DesirealizeFromFile(string path)
        {
            using (FileStream file = File.Open(path, FileMode.Open))
            {
                byte[] bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                Desirialize(bytes, bytes.Length);
                return this;
            }
        }
    }
}
