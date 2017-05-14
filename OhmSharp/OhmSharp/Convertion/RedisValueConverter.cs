using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace OhmSharp.Convertion
{
    /// <summary>
    /// Provides methods to convert to or from RedisValue, and add support for additional types to convert
    /// </summary>
    public static class RedisValueConverter
    {
        /// <summary>
        /// Whether or not <typeparamref name="T"/> can be converted to or from RedisValue
        /// </summary>
        /// <typeparam name="T">type to check if convertable</typeparam>
        /// <returns>true if type is convertable; otherwose false</returns>
        public static bool IsConvertable<T>()
        {
            return IsConvertable(typeof(T));
        }

        /// <summary>
        /// Whether or not <paramref name="type"/> can be converted to or from RedisValue
        /// </summary>
        /// <param name="type">type to check if convertable</param>
        /// <returns>true if type is convertable; otherwose false</returns>
        public static bool IsConvertable(Type type)
        {
            return _customConverters.ContainsKey(type) || _buildinConverters.ContainsKey(type) ||
                EnumConverter.IsEnum(type) || NullableEnumConverter.IsNullableEnum(type);
        }

        /// <summary>
        /// Convert <paramref name="value"/> of type <typeparamref name="T"/> to RedisValue
        /// </summary>
        /// <typeparam name="T">type of value to convert</typeparam>
        /// <param name="value">value to convert to RedisValue</param>
        /// <param name="provider">optional provider controls how value is converted</param>
        /// <returns>RedisValue contains the underlying value</returns>
        /// <exception cref="OhmSharpConvertionException">throw if convertion failed</exception>
        public static RedisValue ConvertTo<T>(T value, IFormatProvider provider = null)
        {
            return ConvertTo(value, typeof(T), provider);
        }

        /// <summary>
        /// Convert <paramref name="value"/> of type <paramref name="type"/> to RedisValue
        /// </summary>
        /// <param name="value">value to convert to RedisValue</param>
        /// <param name="type">type of value to convert</param>
        /// <param name="provider">optional provider controls how value is converted</param>
        /// <returns>RedisValue contains the underlying value</returns>
        /// <exception cref="ArgumentNullException">throw if <paramref name="type"/> is null</exception>
        /// <exception cref="OhmSharpConvertionException">throw if convertion failed</exception>
        public static RedisValue ConvertTo(object value, Type type, IFormatProvider provider = null)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            try
            {
                if (_customConverters.ContainsKey(type))
                {
                    return _customConverters[type].ConvertTo(value, provider);
                }

                if (_buildinConverters.ContainsKey(type))
                {
                    return _buildinConverters[type].ConvertTo(value, provider);
                }

                if (EnumConverter.IsEnum(type))
                {
                    return EnumConverter.ConvertTo(value, type, provider);
                }

                if (NullableEnumConverter.IsNullableEnum(type))
                {
                    return NullableEnumConverter.ConvertTo(value, type, provider);
                }
            }
            catch (Exception ex)
            {
                throw new OhmSharpConvertionException(type,
                    string.Format("Failed to convert type '{0}' to RedisValue.", type.FullName), ex);
            }

            throw new OhmSharpConvertionException(type,
                string.Format("Failed to convert type '{0}' to RedisValue.", type.FullName), 
                new NotSupportedException("No supported IRedisValueConverter found."));
        }

        /// <summary>
        /// Convert <paramref name="value"/> from RedisValue to type <typeparamref name="T"/> 
        /// </summary>
        /// <typeparam name="T">type of object to return</typeparam>
        /// <param name="value">RedisValue to convert from</param>
        /// <param name="provider">optional provider controls how value is converted</param>
        /// <returns>object contained in the RedisValue</returns>
        /// <exception cref="OhmSharpConvertionException">throw if convertion failed</exception>
        public static T ConvertFrom<T>(RedisValue value, IFormatProvider provider = null)
        {
            return (T)ConvertFrom(value, typeof(T), provider);
        }

        /// <summary>
        /// Convert <paramref name="value"/> from RedisValue to type <paramref name="type"/>
        /// </summary>
        /// <param name="value">type of object to return</param>
        /// <param name="type">RedisValue to convert from</param>
        /// <param name="provider">optional provider controls how value is converted</param>
        /// <returns>object contained in the RedisValue</returns>
        /// <exception cref="OhmSharpConvertionException">throw if convertion failed</exception>
        /// <exception cref="ArgumentNullException">throw if <paramref name="type"/> is null</exception>
        public static object ConvertFrom(RedisValue value, Type type, IFormatProvider provider = null)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            try
            {
                if (_customConverters.ContainsKey(type))
                {
                    return _customConverters[type].ConvertFrom(value, provider);
                }

                if (_buildinConverters.ContainsKey(type))
                {
                    return _buildinConverters[type].ConvertFrom(value, provider);
                }

                if (EnumConverter.IsEnum(type))
                {
                    return EnumConverter.ConvertFrom(value, type, provider);
                }

                if (NullableEnumConverter.IsNullableEnum(type))
                {
                    return NullableEnumConverter.ConvertFrom(value, type, provider);
                }
            }
            catch (Exception ex)
            {
                throw new OhmSharpConvertionException(type,
                    string.Format("Failed to convert type '{0}' from RedisValue", type.FullName), ex);
            }

            throw new OhmSharpConvertionException(type,
                string.Format("Failed to convert type '{0}' from RedisValue.", type.FullName), 
                new NotSupportedException("No supported IRedisValueConverter found."));
        }

        /// <summary>
        /// Register a customer IRedisValueConverter used to convert <typeparamref name="T"/> from or to RedisValue
        /// Note that registering custom IRedisValueConverter is intended to occur when startup and is not thread safe
        /// </summary>
        /// <typeparam name="T">type to convert</typeparam>
        /// <param name="converter">cumtomer IRedisValueConverter<T> to registyer</param>
        /// <exception cref="ArgumentNullException">throw if convert is null</exception>
        public static void RegisterConverter<T>(IRedisValueConverter<T> converter)
        {
            RegisterConverter(typeof(T), converter);
        }

        /// <summary>
        /// Register a customer IRedisValueConverter used to convert <paramref name="type"/> from or to RedisValue
        /// Note that registering custom IRedisValueConverter is intended to occur when startup and is not thread safe
        /// </summary>
        /// <param name="type">type to convert</typeparam>
        /// <param name="converter">cumtomer IRedisValueConverter to registyer</param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="type"/> or <paramref name="converter"/> is null</exception>
        public static void RegisterConverter(Type type, IRedisValueConverter converter)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (converter == null)
                throw new ArgumentNullException(nameof(converter));

            RegisterConverter(_customConverters, type, converter);
        }

        /// <summary>
        /// Unregister a customer IRedisValueConverter used to convert <typeparamref name="T"/> from or to RedisValue
        /// </summary>
        /// <typeparam name="T">type to unregister for</typeparam>
        public static void UnregisterConverter<T>()
        {
            UnregisterConverter(typeof(T));
        }

        /// <summary>
        /// Unregister a customer IRedisValueConverter used to convert <paramref name="type"/> from or to RedisValue
        /// </summary>
        /// <param name="type">type to unregister for</param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="type"/> is null</exception>
        public static void UnregisterConverter(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            _customConverters.Remove(type);
        }

        static RedisValueConverter()
        {
            _customConverters = new Dictionary<Type, IRedisValueConverter>();

            _buildinConverters = new Dictionary<Type, IRedisValueConverter>();
            RegisterConverter(_buildinConverters, typeof(string), new StackExchangeRedisValueConverter<string>());
            RegisterConverter(_buildinConverters, typeof(int), new StackExchangeRedisValueConverter<int>());
            RegisterConverter(_buildinConverters, typeof(long), new StackExchangeRedisValueConverter<long>());
            RegisterConverter(_buildinConverters, typeof(double), new StackExchangeRedisValueConverter<double>());
            RegisterConverter(_buildinConverters, typeof(bool), new StackExchangeRedisValueConverter<bool>());
            RegisterConverter(_buildinConverters, typeof(byte[]), new StackExchangeRedisValueConverter<byte[]>());
            RegisterConverter(_buildinConverters, typeof(Guid), new GuidConverter());
            RegisterConverter(_buildinConverters, typeof(DateTime), new DateTimeConverter());
        }

        private static void RegisterConverter(Dictionary<Type, IRedisValueConverter> converters, Type type, IRedisValueConverter converter)
        {
            converters[type] = converter;

            // auto register for Nullable type
            if (type.GetTypeInfo().IsValueType)
            {
                var nullableType = typeof(Nullable<>).MakeGenericType(type);
                var converterType = typeof(NullableTypeConverter<>).MakeGenericType(type);
                var nullableConverter = Activator.CreateInstance(converterType, converter) as IRedisValueConverter;
                converters[nullableType] = nullableConverter;
            }
        }

        // Dictionary is used here as it is thread safe for reading
        private static readonly Dictionary<Type, IRedisValueConverter> _buildinConverters;
        private static readonly Dictionary<Type, IRedisValueConverter> _customConverters;
    }
}
