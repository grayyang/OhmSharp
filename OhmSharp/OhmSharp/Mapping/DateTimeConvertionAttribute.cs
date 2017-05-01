using OhmSharp.Convertion;
using System;
using System.Reflection;

namespace OhmSharp.Mapping
{
    /// <summary>
    /// Specifies the format DateTime is mapped to Redis
    /// </summary>
    public sealed class DateTimeConvertionAttribute : Attribute
    {
        /// <summary>
        /// Convertion info controls the format of DateTime mapped
        /// Default value is DateTimeConvertionInfo.AsLocalTime
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

    internal class DateTimeConvertionAttributeParser : IMemberAttributeParser
    {
        public void Parse(FieldInfo fieldInfo, TypeMetadata typeMetadata, MemberMetadata memberMetadata)
        {
            Parse(fieldInfo.GetCustomAttribute<DateTimeConvertionAttribute>(), typeMetadata, memberMetadata);
        }

        public void Parse(PropertyInfo propertyInfo, TypeMetadata typeMetadata, MemberMetadata memberMetadata)
        {
            Parse(propertyInfo.GetCustomAttribute<DateTimeConvertionAttribute>(), typeMetadata, memberMetadata);
        }

        private void Parse(DateTimeConvertionAttribute attribute, TypeMetadata typeMetadata, MemberMetadata memberMetadata)
        {
            if (attribute != null)
            {
                if ((memberMetadata.Attributes & MemberAttributes.Invalid) == MemberAttributes.Invalid)
                    throw new OhmSharpInvalidSchemaException(typeMetadata.Type, memberMetadata.Name,
                        string.Format("Member {0} of {1} cannot be marked with DateTimeConvertion.", memberMetadata.Name, typeMetadata.Type.FullName));
                if ((memberMetadata.Attributes & MemberAttributes.Ignored) == MemberAttributes.Ignored)
                    throw new OhmSharpInvalidSchemaException(typeMetadata.Type, memberMetadata.Name,
                        string.Format("Member {0} of {1} cannot be marked with both DateTimeConvertion and MappingIgnore.", memberMetadata.Name, typeMetadata.Type.FullName));

                if (!memberMetadata.Type.GetTypeInfo().IsEnum)
                    throw new OhmSharpInvalidSchemaException(typeMetadata.Type,
                        string.Format("Member {0} of {1} not of type DateTime cannot be marked with DateTimeConvertion.", memberMetadata.Name, typeMetadata.Type.FullName));

                memberMetadata.Attributes |= MemberAttributes.Mapped;
                memberMetadata.FormatProvider = attribute.ConvertionInfo;
            }
        }
    }
}
