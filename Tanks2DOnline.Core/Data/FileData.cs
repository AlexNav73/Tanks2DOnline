using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Serialization;
using Tanks2DOnline.Core.Serialization.Attributes;

namespace Tanks2DOnline.Core.Data
{
    public class FileData : SerializableObjectBase
    {
        [Mark] public byte[] Data { get; set; }
        [Mark] private string _fileName = null;

        private readonly string _file = null;

        public FileData() { }
        public FileData(string path) { _file = path; }

        protected override void BeforeSerialization()
        {
            Data = File.ReadAllBytes(_file);
            _fileName = Path.GetFileName(_file);
        }

        protected override void AfterDeserialization()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _fileName);
            File.WriteAllBytes(path, Data);
        }
    }
}
