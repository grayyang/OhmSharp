using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;
using System.Linq.Expressions;

namespace OhmSharp.Convertion
{
    /// <summary>
    /// Wrapper for type conversion available in StackExchange.Redis.RedisValue
    /// </summary>
    internal class StackExchangeRedisValueConverter<T> : IRedisValueConverter<T>, IRedisValueConverter
    {
        public T ConvertFrom(RedisValue value, IFormatProvider provider)
        {
            return _castFrom(value);
        }

        public RedisValue ConvertTo(T value, IFormatProvider provider)
        {
            return _castTo(value);
        }

        object IRedisValueConverter.ConvertFrom(RedisValue value, IFormatProvider provider)
        {
            return this.ConvertFrom(value, provider);
        }

        RedisValue IRedisValueConverter.ConvertTo(object value, IFormatProvider provider)
        {
            return this.ConvertTo((T)value, provider);
        }

        // generate type cast dynamically to avoid static type checking unhappy
        private static Func<TFrom, TTo> Cast<TFrom, TTo>()
        {
            var param = Expression.Parameter(typeof(TFrom));
            var cast = Expression.Convert(param, typeof(TTo));
            return Expression.Lambda<Func<TFrom, TTo>>(cast, param).Compile();
        }

        private readonly Func<RedisValue, T> _castFrom = Cast<RedisValue, T>();
        private readonly Func<T, RedisValue> _castTo = Cast<T, RedisValue>();
    }
}
