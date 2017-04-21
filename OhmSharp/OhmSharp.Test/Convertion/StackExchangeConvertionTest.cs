using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OhmSharp.Convertion;

namespace OhmSharp.Test.Convertion
{
    [TestClass]
    public class StackExchangeConvertionTest : ConverterTestBase<double>
    {
        public StackExchangeConvertionTest()
        {
            Converter = new StackExchangeRedisValueConverter<double>();
        }

        [TestMethod]
        public void ConvertDouble()
        {
            var origin = 1234.5678;
            var converted = Convert(origin);
            Assert.AreEqual(origin, converted);
        }
    }
}
