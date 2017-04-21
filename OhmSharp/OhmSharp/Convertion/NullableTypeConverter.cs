using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;

namespace OhmSharp.Convertion
{
    /// <summary>
    /// IRedisValueConverter for the given <typeparamref name="T"/>'s Nullable type using its IRedisValueConverter
    /// </summary>
    /// <typeparam name="T">ValueType whose Nullable to convert</typeparam>
    internal class NullableTypeConverter<T> : IRedisValueConverter<T?>, IRedisValueConverter where T : struct
    {
        private readonly IRedisValueConverter<T> _valueTypeConverter;

        public NullableTypeConverter(IRedisValueConverter<T> valueTypeConverter)
        {
            _valueTypeConverter = valueTypeConverter;
        }

        public T? ConvertFrom(RedisValue value, IFormatProvider provider)
        {
            return value.IsNull ? null : new Nullable<T>(_valueTypeConverter.ConvertFrom(value, provider));
        }

        public RedisValue ConvertTo(T? value, IFormatProvider provider)
        {
            return value.HasValue ? _valueTypeConverter.ConvertTo(value.Value, provider) : RedisValue.Null;
        }

        object IRedisValueConverter.ConvertFrom(RedisValue value, IFormatProvider provider)
        {
            return this.ConvertFrom(value, provider);
        }

        RedisValue IRedisValueConverter.ConvertTo(object value, IFormatProvider provider)
        {
            return this.ConvertTo((T?)value, provider);
        }
    }
}
