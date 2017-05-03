using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OhmSharp.Mapping;

namespace OhmSharp.Test.Mapping
{
    [TestClass]
    public class MappingAttributesTest
    {
        [TestMethod]
        public void ParseMemberMarkedMappingMember()
        {
            var metadata = MetadataParser.Parse(typeof(TestClassWithMappingMember));

            var member = metadata.TypeMembers.First(m => m.Name == "MemberMarkedMappingMember");
            Assert.AreEqual(MemberAttributes.Property | MemberAttributes.Mapped, member.Attributes);
            Assert.AreEqual(member.Name, member.MemberName);

            var memberWithOverride = metadata.TypeMembers.First(m => m.Name == "MemberMarkedMappingMemberOverride");
            Assert.AreEqual(MemberAttributes.Property | MemberAttributes.Mapped, memberWithOverride.Attributes);
            Assert.AreEqual("OverrideMemberName", memberWithOverride.MemberName);
        }

        [TestMethod]
        public void ParseMemberMarkedMappingIgnore()
        {
            var metadata = MetadataParser.Parse(typeof(TestClassWithMappingIgnore));

            var member = metadata.TypeMembers.First(m => m.Name == "MemberMarkedMappingIgnore");
            Assert.AreEqual(MemberAttributes.Property | MemberAttributes.Ignored, member.Attributes);

            var staticMember = metadata.TypeMembers.First(m => m.Name == "StaticMemberMarkedMappingIgnore");
            Assert.AreEqual(MemberAttributes.Property | MemberAttributes.Ignored | MemberAttributes.Invalid, staticMember.Attributes);
        }

        [TestMethod]
        public void ParseInvalidMemberMarkedMappingMemberShouldThrow()
        {
            Assert.ThrowsException<OhmSharpInvalidSchemaException>(() => MetadataParser.Parse(typeof(TestClassWithInvalidMappingMember)));
        }

        [TestMethod]
        public void ParseMemberMarkedMappingMemberAndMappingIgnoreShouldThrow()
        {
            Assert.ThrowsException<OhmSharpInvalidSchemaException>(() => MetadataParser.Parse(typeof(TestClassWithMappingMemberAndMappingIgnore)));
        }
    }

    public class TestClassWithMappingMember
    {
        [MappingMember]
        public int MemberMarkedMappingMember { get; set; }
        [MappingMember("OverrideMemberName")]
        public int MemberMarkedMappingMemberOverride { get; set; }
    }

    public class TestClassWithMappingIgnore
    {
        [MappingIgnore]
        public int MemberMarkedMappingIgnore { get; set; }
        [MappingIgnore]
        public static int StaticMemberMarkedMappingIgnore { get; set; }
    }

    public class TestClassWithInvalidMappingMember
    {
        [MappingMember]
        public static int InvalidMemberMarkedMappingMember { get; set; }
    }

    public class TestClassWithMappingMemberAndMappingIgnore
    {
        [MappingMember]
        [MappingIgnore]
        public int MemberMarkedMappingMemberAndMappingIgnore { get; set; }
    }
}
