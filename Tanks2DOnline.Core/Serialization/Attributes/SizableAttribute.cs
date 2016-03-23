using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks2DOnline.Core.Serialization.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SizableAttribute : Attribute
    {
        public DataSize Size { get; set; }

        public SizableAttribute(DataSize size)
        {
            Size = size;
        }
    }
}
