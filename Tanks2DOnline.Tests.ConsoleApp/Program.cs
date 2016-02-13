using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tanks2DOnline.Core.Net.Serialization;
using Tanks2DOnline.Tests.Tests.TestEntities;

namespace Tanks2DOnline.Tests.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Serializable obj = new Serializable();
            obj.Init();

            ObjectState s = obj.GetState();

            Serializable obj2 = new Serializable();
            obj2.SetState(s);

            Console.ReadKey();
        }
    }
}
