using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OhmSharp.Convertion;
using OhmSharp.Convertion.Extension;
using StackExchange.Redis;

namespace OhmSharp.Test.Convertion
{
    [TestClass]
    public class RedisValueConverterTest
    {
        [TestMethod]
        public void CheckConvertable()
        {
            RedisValueConverter.RegisterConverter(typeof(object), new MockCustomerConverter());
            Assert.IsTrue(RedisValueConverter.IsConvertable<int>());
            Assert.IsTrue(RedisValueConverter.IsConvertable<int?>());
            Assert.IsTrue(RedisValueConverter.IsConvertable<TestEnum>());
            Assert.IsTrue(RedisValueConverter.IsConvertable<TestEnum?>());
            Assert.IsTrue(RedisValueConverter.IsConvertable<object>());

            RedisValueConverter.UnregisterConverter<object>();
            Assert.IsFalse(RedisValueConverter.IsConvertable<object>());
        }

        [TestMethod]
        public void ConvertBuildinTypes()
        {
            var origin = Guid.NewGuid();
            var converted = Convert(origin);

            Assert.AreEqual(origin, converted);
        }

        [TestMethod]
        public void ConvertEnum()
        {
            var origin = TestEnum.Enum2;
            var converted = Convert(origin);

            Assert.AreEqual(origin, converted);
        }

        [TestMethod]
        public void ConvertNullableEnum()
        {
            Assert.IsNull(Convert<TestEnum?>(null));

            TestEnum? origin = TestEnum.Enum2;
            var converted = Convert(origin);

            Assert.AreEqual(origin, converted);
        }

        [TestMethod]
        public void ConvertTypeWithCustomConverter()
        {
            RedisValueConverter.RegisterConverter(new MockCustomerConverter());

            RedisValue redisVal;
            Guid guid = Convert(Guid.NewGuid(), out redisVal);

            Assert.AreEqual(MockCustomerConverter.MockGuidValue, guid);
            Assert.AreEqual(MockCustomerConverter.MockRedisValue, redisVal);

            RedisValueConverter.UnregisterConverter<Guid>();
        }

        [TestMethod]
        public void ConvertTypeUnregisteredShouldThrow()
        {
            Assert.ThrowsException<OhmSharpConvertionException>(() => RedisValueConverter.ConvertTo(new object()));
            Assert.ThrowsException<OhmSharpConvertionException>(() => RedisValueConverter.ConvertFrom<object>(RedisValue.EmptyString));
        }

        [TestMethod]
        public void ConvertExtentionMethod()
        {
            int origin = 0xFF;
            RedisValue redisVal = origin.ToRedisValue();
            int converted = redisVal.To<int>();

            Assert.IsFalse(redisVal.IsNullOrEmpty);
            Assert.AreEqual(origin, converted);
        }

        private T Convert<T>(T origin)
        {
            RedisValue redisVal;
            return Convert(origin, out redisVal);
        }

        private T Convert<T>(T origin, out RedisValue redisVal)
        {
            redisVal = RedisValueConverter.ConvertTo(origin);
            return RedisValueConverter.ConvertFrom<T>(redisVal);
        }
    }

    public class MockCustomerConverter : IRedisValueConverter<Guid>
    {
        public static Guid MockGuidValue = Guid.Parse("EB9B20A5-128F-48CA-926F-C6844D271CA7");

        public static RedisValue MockRedisValue = "MockGuidValue";

        public Guid ConvertFrom(RedisValue value, IFormatProvider provider)
        {
            return MockGuidValue;
        }

        public RedisValue ConvertTo(Guid value, IFormatProvider provider)
        {
            return MockRedisValue;
        }

        RedisValue IRedisValueConverter.ConvertTo(object value, IFormatProvider provider)
        {
            return this.ConvertTo((Guid)value, provider);
        }

        object IRedisValueConverter.ConvertFrom(RedisValue value, IFormatProvider provider)
        {
            return this.ConvertFrom(value, provider);
        }
    }
}
