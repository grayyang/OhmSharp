using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OhmSharp.Convertion;

namespace OhmSharp.Test.Convertion
{
    [TestClass]
    public class DateTimeConvertionTest : ConverterTestBase<DateTime>
    {
        public DateTimeConvertionTest()
        {
            Converter = new DateTimeConverter();
        }

        [TestMethod]
        public void ConvertDateTimeUsingDefaultConvertion()
        {
            var origin = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
            var converted = Convert(origin);

            Assert.AreEqual(origin, converted);
            Assert.AreEqual(DateTimeKind.Local, converted.Kind);
        }

        [TestMethod]
        public void ConvertDateTimeUsingLocalKind()
        {
            var origin = DateTime.UtcNow;
            var converted = Convert(origin, DateTimeConvertionInfo.AsLocalTime);

            Assert.AreEqual(origin.ToLocalTime(), converted);
            Assert.AreEqual(DateTimeKind.Local, converted.Kind);
        }

        [TestMethod]
        public void ConvertDateTimeUsingUtcKind()
        {
            var origin = DateTime.Now;
            var converted = Convert(origin, DateTimeConvertionInfo.AsUtcTime);

            Assert.AreEqual(origin.ToUniversalTime(), converted);
            Assert.AreEqual(DateTimeKind.Utc, converted.Kind);
        }

        [TestMethod]
        public void ConvertDateTimeUsingDateOnly()
        {
            var origin = DateTime.Now.Date;
            var converted = Convert(origin, DateTimeConvertionInfo.AsDate);

            Assert.AreEqual(origin, converted);
            Assert.AreEqual(DateTimeKind.Unspecified, converted.Kind);
        }
    }
}
