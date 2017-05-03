using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OhmSharp.Mapping;

namespace OhmSharp.Test.Mapping
{
    [TestClass]
    public class TypeParserTest
    {
        [TestMethod]
        public void ParsePublicClass()
        {
            var metadate = MetadataParser.Parse(typeof(TestPublicClass));

            Assert.AreEqual(typeof(TestPublicClass), metadate.Type);
            Assert.AreEqual(typeof(TestPublicClass).FullName, metadate.KeyName);
            Assert.AreEqual(false, metadate.Concurrency);
        }

        [TestMethod]
        public void ParseClassMarkedMappingObject()
        {
            var metadate = MetadataParser.Parse(typeof(TestClassMarkedMappingObject));

            Assert.AreEqual(typeof(TestClassMarkedMappingObject), metadate.Type);
            Assert.AreEqual(typeof(TestClassMarkedMappingObject).FullName, metadate.KeyName);
            Assert.AreEqual(false, metadate.Concurrency);

            metadate = MetadataParser.Parse(typeof(TestClassMarkedMappingObjectOverride));

            Assert.AreEqual(typeof(TestClassMarkedMappingObjectOverride), metadate.Type);
            Assert.AreEqual("OverrideKeyName", metadate.KeyName);
            Assert.AreEqual(true, metadate.Concurrency);
        }

        [TestMethod]
        public void ParseClassWithVisibleParameterlessConstructor()
        {
            var metadate = MetadataParser.Parse(typeof(TestClassWithPublicConstructor));

            Assert.AreEqual(typeof(TestClassWithPublicConstructor), metadate.Type);
            Assert.AreEqual(typeof(TestClassWithPublicConstructor).FullName, metadate.KeyName);
            Assert.AreEqual(false, metadate.Concurrency);

            metadate = MetadataParser.Parse(typeof(TestClassWithProtectedConstructor));

            Assert.AreEqual(typeof(TestClassWithProtectedConstructor), metadate.Type);
            Assert.AreEqual(typeof(TestClassWithProtectedConstructor).FullName, metadate.KeyName);
            Assert.AreEqual(false, metadate.Concurrency);
        }

        [TestMethod]
        public void ParseNonPublicClassShouldThrow()
        {
            Assert.ThrowsException<OhmSharpInvalidSchemaException>(() => MetadataParser.Parse(typeof(TestNonPublicClass)));
        }

        [TestMethod]
        public void ParseSealedClassShouldThrow()
        {
            Assert.ThrowsException<OhmSharpInvalidSchemaException>(() => MetadataParser.Parse(typeof(TestSealedClass)));
        }

        [TestMethod]
        public void ParseClassWithoutValidConstructorShouldThrow()
        {
            Assert.ThrowsException<OhmSharpInvalidSchemaException>(() => MetadataParser.Parse(typeof(TestClassWithoutValidConstructor)));
        }

        [TestMethod]
        public void ParseUnsupportedTypeShouldThrow()
        {
            Assert.ThrowsException<OhmSharpInvalidSchemaException>(() => MetadataParser.Parse(typeof(int)));
            Assert.ThrowsException<OhmSharpInvalidSchemaException>(() => MetadataParser.Parse(typeof(int*)));
            Assert.ThrowsException<OhmSharpInvalidSchemaException>(() => MetadataParser.Parse(typeof(int[])));
            Assert.ThrowsException<OhmSharpInvalidSchemaException>(() => MetadataParser.Parse(typeof(ITestInterface)));
            Assert.ThrowsException<OhmSharpInvalidSchemaException>(() => MetadataParser.Parse(typeof(TestEnum)));
        }
    }

    public class TestPublicClass
    { }

    [MappingObject]
    public class TestClassMarkedMappingObject
    { }

    [MappingObject("OverrideKeyName", Concurrency = true)]
    public class TestClassMarkedMappingObjectOverride
    { }

    public class TestClassWithPublicConstructor
    {
        public TestClassWithPublicConstructor()
        { }
    }

    public class TestClassWithProtectedConstructor
    {
        protected TestClassWithProtectedConstructor()
        { }
    }

    public class TestClassWithoutValidConstructor
    {
        private TestClassWithoutValidConstructor()
        { }

        public TestClassWithoutValidConstructor(int param)
        { }
    }

    class TestNonPublicClass
    { }

    public sealed class TestSealedClass
    { }

    public interface ITestInterface
    { }

    public enum TestEnum
    { }
}
