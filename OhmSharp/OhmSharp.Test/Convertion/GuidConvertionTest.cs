using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OhmSharp.Convertion;

namespace OhmSharp.Test.Convertion
{
    [TestClass]
    public class GuidConvertionTest : ConverterTestBase<Guid>
    {
        public GuidConvertionTest()
        {
            Converter = new GuidConverter();
        }

        [TestMethod]
        public void ConvertGuid()
        {
            var origin = Guid.NewGuid();
            var converted = Convert(origin);
            Assert.AreEqual(origin, converted);
        }
    }
}
