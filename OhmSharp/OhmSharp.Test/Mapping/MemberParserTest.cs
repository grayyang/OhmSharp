using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OhmSharp.Mapping;
#pragma warning disable 0169

namespace OhmSharp.Test.Mapping
{
    [TestClass]
    public class MemberParserTest
    {
        [TestMethod]
        public void ParseField()
        {
            var metadata = SchemaParser.Parse(typeof(TestClassWithField));
            Assert.AreEqual(4, metadata.TypeMembers.Count);

            var publicField = metadata.TypeMembers.First(m => m.Name == "PublicField");
            Assert.AreEqual(typeof(int), publicField.Type);
            Assert.AreEqual("PublicField", publicField.MemberName);
            Assert.AreEqual(MemberAttributes.Field, publicField.Attributes);
            Assert.AreEqual(GetterSetterAttributes.Defined, publicField.Getter);
            Assert.AreEqual(GetterSetterAttributes.Defined, publicField.Setter);

            var protectedField = metadata.TypeMembers.First(m => m.Name == "ProtectedField");
            Assert.AreEqual(typeof(int), protectedField.Type);
            Assert.AreEqual("ProtectedField", protectedField.MemberName);
            Assert.AreEqual(MemberAttributes.Field, protectedField.Attributes);
            Assert.AreEqual(GetterSetterAttributes.Defined, protectedField.Getter);
            Assert.AreEqual(GetterSetterAttributes.Defined, protectedField.Setter);

            var privateField = metadata.TypeMembers.First(m => m.Name == "PrivateField");
            Assert.AreEqual(typeof(int), privateField.Type);
            Assert.AreEqual("PrivateField", privateField.MemberName);
            Assert.AreEqual(MemberAttributes.Field | MemberAttributes.Invalid, privateField.Attributes);
            Assert.AreEqual(GetterSetterAttributes.Defined, privateField.Getter);
            Assert.AreEqual(GetterSetterAttributes.Defined, privateField.Setter);

            var staticField = metadata.TypeMembers.First(m => m.Name == "StaticField");
            Assert.AreEqual(typeof(int), staticField.Type);
            Assert.AreEqual("StaticField", staticField.MemberName);
            Assert.AreEqual(MemberAttributes.Field | MemberAttributes.Invalid, staticField.Attributes);
            Assert.AreEqual(GetterSetterAttributes.Defined, staticField.Getter);
            Assert.AreEqual(GetterSetterAttributes.Defined, staticField.Setter);
        }

        [TestMethod]
        public void ParseProperty()
        {
            var metadata = SchemaParser.Parse(typeof(TestClassWithProperty));
            Assert.AreEqual(9, metadata.TypeMembers.Count(m => (m.Attributes & MemberAttributes.Property) == MemberAttributes.Property));

            var publicProperty = metadata.TypeMembers.First(m => m.Name == "PublicProperty");
            Assert.AreEqual(typeof(int), publicProperty.Type);
            Assert.AreEqual("PublicProperty", publicProperty.MemberName);
            Assert.AreEqual(MemberAttributes.Property, publicProperty.Attributes);
            Assert.AreEqual(GetterSetterAttributes.Defined, publicProperty.Getter);
            Assert.AreEqual(GetterSetterAttributes.Defined, publicProperty.Setter);

            var protectedProperty = metadata.TypeMembers.First(m => m.Name == "ProtectedProperty");
            Assert.AreEqual(typeof(int), protectedProperty.Type);
            Assert.AreEqual("ProtectedProperty", protectedProperty.MemberName);
            Assert.AreEqual(MemberAttributes.Property, protectedProperty.Attributes);
            Assert.AreEqual(GetterSetterAttributes.Defined, protectedProperty.Getter);
            Assert.AreEqual(GetterSetterAttributes.Defined, protectedProperty.Setter);

            var privateProperty = metadata.TypeMembers.First(m => m.Name == "PrivateProperty");
            Assert.AreEqual(typeof(int), privateProperty.Type);
            Assert.AreEqual("PrivateProperty", privateProperty.MemberName);
            Assert.AreEqual(MemberAttributes.Property | MemberAttributes.Invalid, privateProperty.Attributes);
            Assert.AreEqual(GetterSetterAttributes.None, privateProperty.Getter);
            Assert.AreEqual(GetterSetterAttributes.None, privateProperty.Setter);

            var privateSetProperty = metadata.TypeMembers.First(m => m.Name == "PublicGetPrivateSetProperty");
            Assert.AreEqual(typeof(int), privateSetProperty.Type);
            Assert.AreEqual("PublicGetPrivateSetProperty", privateSetProperty.MemberName);
            Assert.AreEqual(MemberAttributes.Property, privateSetProperty.Attributes);
            Assert.AreEqual(GetterSetterAttributes.Defined, privateSetProperty.Getter);
            Assert.AreEqual(GetterSetterAttributes.None, privateSetProperty.Setter);

            var virtualProperty = metadata.TypeMembers.First(m => m.Name == "VirtualProperty");
            Assert.AreEqual(typeof(int), virtualProperty.Type);
            Assert.AreEqual("VirtualProperty", virtualProperty.MemberName);
            Assert.AreEqual(MemberAttributes.Property, virtualProperty.Attributes);
            Assert.AreEqual(GetterSetterAttributes.Defined | GetterSetterAttributes.Virtual, virtualProperty.Getter);
            Assert.AreEqual(GetterSetterAttributes.Defined | GetterSetterAttributes.Virtual, virtualProperty.Setter);

            var staticProperty = metadata.TypeMembers.First(m => m.Name == "StaticProperty");
            Assert.AreEqual(typeof(int), staticProperty.Type);
            Assert.AreEqual("StaticProperty", staticProperty.MemberName);
            Assert.AreEqual(MemberAttributes.Property | MemberAttributes.Invalid, staticProperty.Attributes);
            Assert.AreEqual(GetterSetterAttributes.None, staticProperty.Getter);
            Assert.AreEqual(GetterSetterAttributes.None, staticProperty.Setter);

            var getOnlyProperty = metadata.TypeMembers.First(m => m.Name == "GetOnlyProperty");
            Assert.AreEqual(typeof(int), getOnlyProperty.Type);
            Assert.AreEqual("GetOnlyProperty", getOnlyProperty.MemberName);
            Assert.AreEqual(MemberAttributes.Property, getOnlyProperty.Attributes);
            Assert.AreEqual(GetterSetterAttributes.Defined, getOnlyProperty.Getter);
            Assert.AreEqual(GetterSetterAttributes.None, getOnlyProperty.Setter);

            var setOnlyProperty = metadata.TypeMembers.First(m => m.Name == "SetOnlyProperty");
            Assert.AreEqual(typeof(int), setOnlyProperty.Type);
            Assert.AreEqual("SetOnlyProperty", setOnlyProperty.MemberName);
            Assert.AreEqual(MemberAttributes.Property | MemberAttributes.Invalid, setOnlyProperty.Attributes);
            Assert.AreEqual(GetterSetterAttributes.None, setOnlyProperty.Getter);
            Assert.AreEqual(GetterSetterAttributes.Defined, setOnlyProperty.Setter);

            var indexerProperty = metadata.TypeMembers.First(m => m.Name == "Item");
            Assert.AreEqual(MemberAttributes.Property | MemberAttributes.Invalid, indexerProperty.Attributes);
            Assert.AreEqual(GetterSetterAttributes.Defined, indexerProperty.Getter);
            Assert.AreEqual(GetterSetterAttributes.Defined, indexerProperty.Setter);
        }
    }

    public class TestClassWithField
    {
        public int PublicField;
        protected int ProtectedField;
        private int PrivateField;
        public static int StaticField;
    }

    public class TestClassWithProperty
    {
        public int PublicProperty { get; set; }
        protected int ProtectedProperty { get; set; }
        private int PrivateProperty { get; set; }
        public int PublicGetPrivateSetProperty { get; private set; }
        public virtual int VirtualProperty { get; set; }
        public static int StaticProperty { get; set; }
        public int GetOnlyProperty { get { return 0; } }
        public int SetOnlyProperty { set { } }
        public object this[int n]
        {
            get { return null; }
            set { }
        }
    }
}
