using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;

namespace OhmSharp.Convertion
{
    /// <summary>
    /// IRedisValueConverter for Guid type
    /// </summary>
    internal class GuidConverter : IRedisValueConverter<Guid>, IRedisValueConverter
    {
        public Guid ConvertFrom(RedisValue value, IFormatProvider provider)
        {
            return new Guid((byte[])value);
        }

        public RedisValue ConvertTo(Guid value, IFormatProvider provider)
        {
            return value.ToByteArray();
        }

        object IRedisValueConverter.ConvertFrom(RedisValue value, IFormatProvider provider)
        {
            return this.ConvertFrom(value, provider);
        }

        RedisValue IRedisValueConverter.ConvertTo(object value, IFormatProvider provider)
        {
            return this.ConvertTo((Guid)value, provider);
        }
    }
}
