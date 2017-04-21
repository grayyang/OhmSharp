using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;
using System.Globalization;

namespace OhmSharp.Convertion
{
    /// <summary>
    /// IRedisValueConverter for DateTime type
    /// </summary>
    internal class DateTimeConverter : IRedisValueConverter<DateTime>, IRedisValueConverter
    {
        public DateTime ConvertFrom(RedisValue value, IFormatProvider provider)
        {
            var dateOnly = false;
            var kind = DateTimeKind.Local;

            var formatInfo = provider?.GetFormat(typeof(DateTimeConvertionInfo)) as DateTimeConvertionInfo;
            if (formatInfo != null)
            {
                kind = formatInfo.Kind;
                dateOnly = formatInfo.DateOnly;
            }

            var dateTime = DateTime.SpecifyKind(
                DateTime.ParseExact((string)value, dateOnly ? DateOnlyFormat : DateTimeFormat, CultureInfo.InvariantCulture),
                dateOnly ? DateTimeKind.Unspecified : DateTimeKind.Utc);

            if (dateOnly == false && kind == DateTimeKind.Local)
                return dateTime.ToLocalTime();
            else
                return dateTime;
        }

        public RedisValue ConvertTo(DateTime value, IFormatProvider provider)
        {
            var dateOnly = false;

            var formatInfo = provider?.GetFormat(typeof(DateTimeConvertionInfo)) as DateTimeConvertionInfo;
            if (formatInfo != null)
                dateOnly = formatInfo.DateOnly;

            if (dateOnly)
                return value.ToString(DateOnlyFormat);
            else
                return value.ToUniversalTime().ToString(DateTimeFormat);
        }

        object IRedisValueConverter.ConvertFrom(RedisValue value, IFormatProvider provider)
        {
            return this.ConvertFrom(value, provider);
        }

        RedisValue IRedisValueConverter.ConvertTo(object value, IFormatProvider provider)
        {
            return this.ConvertTo((DateTime)value, provider);
        }

        private static readonly string DateOnlyFormat = @"yyyyy-MM-dd";
        private static readonly string DateTimeFormat = @"yyyyy-MM-ddTHH\:mm\:ss.fffffff";
    }

    /// <summary>
    /// IFormatProvider provides control on how DateTime is stored in RedisValue
    /// </summary>
    public class DateTimeConvertionInfo : IFormatProvider
    {
        /// <summary>
        /// Convert DateTime to or from RedisValue with only date included
        /// </summary>
        public static readonly DateTimeConvertionInfo AsDate = new DateTimeConvertionInfo(DateTimeKind.Unspecified, true);

        /// <summary>
        /// Convert DateTime from RedisValue as Utc time
        /// </summary>
        public static readonly DateTimeConvertionInfo AsUtcTime = new DateTimeConvertionInfo(DateTimeKind.Utc, false);

        /// <summary>
        /// Convert DateTime from RedisValue as local time
        /// </summary>
        public static readonly DateTimeConvertionInfo AsLocalTime = new DateTimeConvertionInfo(DateTimeKind.Local, false);

        /// <summary>
        /// Whether only date is converted  
        /// </summary>
        public bool DateOnly { get; private set; }

        /// <summary>
        /// Convert DateTime back as Utc time or local time
        /// </summary>
        public DateTimeKind Kind { get; private set; }

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(DateTimeConvertionInfo))
                return this;
            else
                return null;
        }

        private DateTimeConvertionInfo(DateTimeKind kind, bool dateOnly)
        {
            Kind = kind;
            DateOnly = dateOnly;
        }
    }
}
