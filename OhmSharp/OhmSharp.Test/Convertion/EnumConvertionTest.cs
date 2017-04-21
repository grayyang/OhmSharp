using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OhmSharp.Convertion;
using StackExchange.Redis;

namespace OhmSharp.Test.Convertion
{
    [TestClass]
    public class EnumConvertionTest
    {
        [TestMethod]
        public void ConvertEnumUsingDefaultRepresent()
        {
            TestEnum originEnum = TestEnum.Enum2;
            TestFlag originFlag = TestFlag.Flag1 | TestFlag.Flag2;

            RedisValue redisValEnum;
            var convertedEnum = Convert(originEnum, null, out redisValEnum);
            Assert.AreEqual(originEnum, convertedEnum);
            Assert.IsFalse(IsNumeric(redisValEnum));

            RedisValue redisValFlag;
            var convertedFlag = Convert(originFlag, null, out redisValFlag);
            Assert.AreEqual(originFlag, convertedFlag);
            Assert.IsFalse(IsNumeric(redisValFlag));
        }

        [TestMethod]
        public void ConvertEnumUsingStringRepresent()
        {
            TestEnum originEnum = TestEnum.Enum2;
            TestFlag originFlag = TestFlag.Flag1 | TestFlag.Flag2;

            RedisValue redisValEnum;
            var convertedEnum = Convert(originEnum, EnumConvertionInfo.AsString, out redisValEnum);
            Assert.AreEqual(originEnum, convertedEnum);
            Assert.IsFalse(IsNumeric(redisValEnum));

            RedisValue redisValFlag;
            var convertedFlag = Convert(originFlag, EnumConvertionInfo.AsString, out redisValFlag);
            Assert.AreEqual(originFlag, convertedFlag);
            Assert.IsFalse(IsNumeric(redisValFlag));
        }

        [TestMethod]
        public void ConvertEnumUsingNumericRepresent()
        {
            TestEnum originEnum = TestEnum.Enum2;
            TestFlag originFlag = TestFlag.Flag1 | TestFlag.Flag2;

            RedisValue redisValEnum;
            var convertedEnum = Convert(originEnum, EnumConvertionInfo.AsNumeric, out redisValEnum);
            Assert.AreEqual(originEnum, convertedEnum);
            Assert.IsTrue(IsNumeric(redisValEnum));

            RedisValue redisValFlag;
            var convertedFlag = Convert(originFlag, EnumConvertionInfo.AsNumeric, out redisValFlag);
            Assert.AreEqual(originFlag, convertedFlag);
            Assert.IsTrue(IsNumeric(redisValFlag));
        }

        private T Convert<T>(T origin, IFormatProvider provider, out RedisValue redisVal)
        {
            redisVal = EnumConverter.ConvertTo(origin, typeof(T), provider);
            return (T)EnumConverter.ConvertFrom(redisVal, typeof(T), provider);
        }

        private bool IsNumeric(RedisValue value)
        {
            return value.ToString().All(c => char.IsDigit(c));
        }
    }

    enum TestEnum
    {
        Enum1,
        Enum2,
    }

    [Flags]
    enum TestFlag : uint
    {
        Flag1 = 0x1,
        Flag2 = 0x10,
    }
}
