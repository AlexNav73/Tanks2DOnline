using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tanks2DOnline.Core.Net.CommonData;
using Tanks2DOnline.Core.Serialization.Attributes;
using Tanks2DOnline.Tests.Tests.TestEntities;

namespace Tanks2DOnline.Tests.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SerializationToFileTest()
        {
            Serializable obj = new Serializable();
            obj.Init();

            obj.SerializeToFile("object.dat");

            obj.PropString = "New String";
            obj.Inner.Type = PacketType.Registration;

            Serializable obj2 = obj.DesirealizeFromFile("object.dat");

            Assert.AreEqual(obj2.PropBool, true);
            Assert.AreEqual(obj2.PropInt, 42);
            Assert.AreEqual(obj2.PropString, "Fuck you!");
            Assert.AreEqual(obj2.Inner.Type, PacketType.HoldsData);
        }
    }
}
