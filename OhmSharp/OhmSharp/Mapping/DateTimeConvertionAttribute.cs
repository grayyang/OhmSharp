﻿using OhmSharp.Convertion;
using System;

namespace OhmSharp.Mapping
{
    /// <summary>
    /// Specifies the format DateTime is mapped to Redis
    /// </summary>
    public sealed class DateTimeConvertionAttribute : Attribute
    {
        /// <summary>
        /// Convertion info controls the format of DateTime mapped
        /// Default value is <see cref="DateTimeConvertionInfo.AsLocalTime"/>
        /// </summary>
        public DateTimeConvertionInfo ConvertionInfo { get; private set; }

        /// <summary>
        /// Constructs a new DateTimeConvertionAttribute with default convertion options
        /// </summary>
        public DateTimeConvertionAttribute()
            : this(DateTimeConvertionInfo.AsLocalTime)
        { }

        /// <summary>
        /// Constructs a new DateTimeConvertionAttribute with specified convertion options
        /// </summary>
        /// <param name="convertionInfo">DateTimeConvertionInfo controls the format of DateTime converted</param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="convertionInfo"/> is null</exception>
        public DateTimeConvertionAttribute(DateTimeConvertionInfo convertionInfo)
        {
            ConvertionInfo = convertionInfo ?? throw new ArgumentNullException(nameof(convertionInfo));
        }
    }

    internal class DateTimeConvertionAttributeParser : MemberAttributeParser<DateTimeConvertionAttribute>
    {
        protected override void Parse(DateTimeConvertionAttribute attribute, TypeMetadata typeMetadata, MemberMetadata memberMetadata)
        {
            if (attribute != null)
            {
                if ((memberMetadata.Attributes & MemberAttributes.Unmappable) == MemberAttributes.Unmappable)
                    throw new OhmSharpInvalidSchemaException(typeMetadata.Type, memberMetadata.Name,
                        string.Format("Member {0} of {1} cannot be marked with DateTimeConvertion.", memberMetadata.Name, typeMetadata.Type.FullName));
                if ((memberMetadata.Attributes & MemberAttributes.Ignored) == MemberAttributes.Ignored)
                    throw new OhmSharpInvalidSchemaException(typeMetadata.Type, memberMetadata.Name,
                        string.Format("Member {0} of {1} cannot be marked with both DateTimeConvertion and MappingIgnore.", memberMetadata.Name, typeMetadata.Type.FullName));

                if (memberMetadata.Type != typeof(DateTime) && memberMetadata.Type != typeof(DateTime?))
                    throw new OhmSharpInvalidSchemaException(typeMetadata.Type,
                        string.Format("Member {0} of {1} not of type DateTime cannot be marked with DateTimeConvertion.", memberMetadata.Name, typeMetadata.Type.FullName));

                if (memberMetadata == typeMetadata.ConcurrencyMember && attribute.ConvertionInfo.DateOnly)
                    throw new OhmSharpInvalidSchemaException(typeMetadata.Type, memberMetadata.Name,
                        string.Format("Member {0} of {1} marked with MappingConcurrency cannot use DateTimeConvertionInfo.AsDate.", memberMetadata.Name, typeMetadata.Type.FullName));

                memberMetadata.Attributes |= MemberAttributes.Mapped;
                memberMetadata.FormatProvider = attribute.ConvertionInfo;
            }
        }
    }
}
