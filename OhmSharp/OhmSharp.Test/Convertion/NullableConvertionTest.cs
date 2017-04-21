using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OhmSharp.Convertion;

namespace OhmSharp.Test.Convertion
{
    [TestClass]
    public class NullableConvertionTest : ConverterTestBase<Guid?>
    {
        public NullableConvertionTest()
        {
            Converter = new NullableTypeConverter<Guid>(new GuidConverter());
        }

        [TestMethod]
        public void ConvertNullableType()
        {
            var origin = Guid.NewGuid();
            var converted = Convert(origin);
            Assert.IsNotNull(converted);
            Assert.AreEqual(origin, converted);

            var nil = Convert(null);
            Assert.IsNull(nil);
        }
    }
}
