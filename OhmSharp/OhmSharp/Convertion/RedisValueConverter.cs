﻿using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace OhmSharp.Convertion
{
    /// <summary>
    /// Entry class 
    /// </summary>
    public static class RedisValueConverter
    {
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
        /// <exception cref="OhmSharpConvertionException">throw if convertion failed</exception>
        /// <exception cref="ArgumentNullException">throw if type is null</exception>
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

                if (type.GetTypeInfo().IsEnum)
                {
                    return EnumConverter.ConvertTo(value, type, provider);
                }

                if (_buildinConverters.ContainsKey(type))
                {
                    return _buildinConverters[type].ConvertTo(value, provider);
                }
            }
            catch (Exception ex)
            {
                throw new OhmSharpConvertionException(type,
                    string.Format("Failed to convert type '{0}' to RedisValue.", type.FullName), ex);
            }

            throw new OhmSharpConvertionException(type,
                string.Format("Failed to convert type '{0}' to RedisValue.", type.FullName), new NotSupportedException("No supported IRedisValueConverter found."));
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
        /// <exception cref="ArgumentNullException">throw if type is null</exception>
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

                if (type.GetTypeInfo().IsEnum)
                {
                    return EnumConverter.ConvertFrom(value, type, provider);
                }

                if (_buildinConverters.ContainsKey(type))
                {
                    return _buildinConverters[type].ConvertFrom(value, provider);
                }
            }
            catch (Exception ex)
            {
                throw new OhmSharpConvertionException(type,
                    string.Format("Failed to convert type '{0}' from RedisValue", type.FullName), ex);
            }

            throw new OhmSharpConvertionException(type,
                string.Format("Failed to convert type '{0}' from RedisValue since no supported IRedisValueConverter found.", type.FullName));
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
        /// <exception cref="ArgumentNullException">throw if type or convert is null</exception>
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
        /// <exception cref="ArgumentNullException">throw if type is null</exception>
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