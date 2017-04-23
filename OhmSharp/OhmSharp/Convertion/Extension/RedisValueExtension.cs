using StackExchange.Redis;
using System;

namespace OhmSharp.Convertion.Extension
{
    /// <summary>
    /// Provides extenstion methods for RedisValue convertion
    /// </summary>
    public static class RedisValueExtension
    {
        /// <summary>
        /// Convert RedisValue to type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">type of object to return</typeparam>
        /// <param name="value">RedisValue to convert</param>
        /// <param name="provider">optional provider controls how value is converted</param>
        /// <returns>object contained in the RedisValue</returns>
        /// <exception cref="OhmSharpConvertionException">throw if convertion failed</exception>
        public static T To<T>(this RedisValue value, IFormatProvider provider = null)
        {
            return RedisValueConverter.ConvertFrom<T>(value, provider);
        }

        /// <summary>
        /// Convert object to RedisValue if supported
        /// </summary>
        /// <param name="value">object to convert</param>
        /// <param name="provider">optional provider controls how value is converted</param>
        /// <returns>RedisValue contains content of the object</returns>
        /// <exception cref="OhmSharpConvertionException">throw if convertion failed</exception>
        public static RedisValue ToRedisValue(this object value, IFormatProvider provider = null)
        {
            return RedisValueConverter.ConvertTo(value, value.GetType(), provider);
        }
    }
}
