using StackExchange.Redis;
using System;

namespace OhmSharp.Convertion
{
    /// <summary>
    /// Converter that converts enum to or from RedisValue
    /// </summary>
    internal static class EnumConverter
    {
        /// <summary>
        /// Convert enum to RedisValue
        /// </summary>
        /// <param name="value">enum value to convert</param>
        /// <param name="type">enum type to convert</param>
        /// <param name="provider">provider controls how enum value is converted</param>
        /// <returns>RedisValue represents enum value</returns>
        public static RedisValue ConvertTo(object value, Type type, IFormatProvider provider)
        {
            var format = "g";
            var formatInfo = provider?.GetFormat(typeof(EnumConvertionInfo)) as EnumConvertionInfo;
            if (formatInfo != null && formatInfo.Convertion == EnumConvertion.AsNumeric)
                format = "d";

            return Enum.Format(type, value, format);
        }

        /// <summary>
        /// Convert enum from RedisValue
        /// </summary>
        /// <param name="value">enum value to convert</param>
        /// <param name="type">enum type to convert</param>
        /// <param name="provider">provider controls how enum value is converted</param>
        /// <returns>enum value contained in RedisValue</returns>
        public static object ConvertFrom(RedisValue value, Type type, IFormatProvider provider)
        {
            return Enum.Parse(type, value);
        }
    }

    /// <summary>
    /// IFormatProvider provides control on how enum is stored in RedisValue
    /// </summary>
    public class EnumConvertionInfo : IFormatProvider
    {
        /// <summary>
        /// Convert enum to or from RedisValue as string
        /// </summary>
        public static readonly EnumConvertionInfo AsString = new EnumConvertionInfo(EnumConvertion.AsString);

        /// <summary>
        /// Convert enum to or from RedisValue as numeric value
        /// </summary>
        public static readonly EnumConvertionInfo AsNumeric = new EnumConvertionInfo(EnumConvertion.AsNumeric);

        /// <summary>
        /// Convert enum to RedisValue as string or numeric
        /// </summary>
        public EnumConvertion Convertion { get; private set; }

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(EnumConvertionInfo))
                return this;
            else
                return null;
        }

        private EnumConvertionInfo(EnumConvertion convertion)
        {
            Convertion = convertion;
        }
    }

    /// <summary>
    /// Control how enum value is stored in RedisValue
    /// </summary>
    public enum EnumConvertion
    {
        /// <summary>
        /// Save enum using string representation
        /// </summary>
        AsString,

        /// <summary>
        /// Save enum using numeric representation
        /// </summary>
        AsNumeric,
    }
}
