using StackExchange.Redis;
using System;
#pragma warning disable 0108

namespace OhmSharp.Convertion
{
    /// <summary>
    /// Defines methods that converts type <typeparamref name="T"/> to or from RedisValue
    /// </summary>
    /// <typeparam name="T">type to convert</typeparam>
    public interface IRedisValueConverter<T> : IRedisValueConverter
    {
        /// <summary>
        /// Convert <paramref name="value"/> to <typeparamref name="T"/>
        /// </summary>
        /// <param name="value">value to convert from RedisValue</param>
        /// <param name="provider">object that controls the formatting</param>
        /// <returns>object contained in the RedisValue</returns>
        T ConvertFrom(RedisValue value, IFormatProvider provider);
        /// <summary>
        /// Convert <paramref name="value"/> to RedisValue
        /// </summary>
        /// <param name="value">value to convert to RedisValue</param>
        /// <param name="provider">object that controls the formatting</param>
        /// <returns>RedisValue contains the underlying value</returns>
        RedisValue ConvertTo(T value, IFormatProvider provider);
    }

    /// <summary>
    /// Defines methods that converts object to or from RedisValue
    /// </summary>
    public interface IRedisValueConverter
    {
        /// <summary>
        /// Convert <paramref name="value"/> to object
        /// </summary>
        /// <param name="value">value to convert from RedisValue</param>
        /// <param name="provider">object that controls the formatting</param>
        /// <returns>object contained in the RedisValue</returns>
        object ConvertFrom(RedisValue value, IFormatProvider provider);

        /// <summary>
        /// Convert object to RedisValue
        /// </summary>
        /// <param name="value">value to convert to RedisValue</param>
        /// <param name="provider">object that controls the formatting</param>
        /// <returns>RedisValue contains the underlying value</returns>
        RedisValue ConvertTo(object value, IFormatProvider provider);
    }
}
