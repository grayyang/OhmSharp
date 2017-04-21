using OhmSharp.Convertion;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace OhmSharp.Test.Convertion
{
    public class ConverterTestBase<T>
    {
        protected IRedisValueConverter<T> Converter;

        protected T Convert(T origin, IFormatProvider provider, out RedisValue redisVal)
        {
            redisVal = Converter.ConvertTo(origin, provider);
            return Converter.ConvertFrom(redisVal, provider);
        }

        protected T Convert(T origin, IFormatProvider provider = null)
        {
            RedisValue redisVal;
            return Convert(origin, provider, out redisVal);
        }
    }
}
