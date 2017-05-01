﻿using OhmSharp.Convertion;
using System;
using System.Reflection;

namespace OhmSharp.Mapping
{
    /// <summary>
    /// Specifies the format Enum is mapped to Redis
    /// </summary>
    public sealed class EnumConvertionAttribute : Attribute
    {
        /// <summary>
        /// Convertion info controls the format of Enum mapped
        /// Default value
        /// </summary>
        public EnumConvertionInfo ConvertionInfo { get; private set; }

        /// <summary>
        /// Constructs a new EnumConvertionAttribute with default convertion options
        /// </summary>
        public EnumConvertionAttribute()
            : this(EnumConvertionInfo.AsString)
        { }

        /// <summary>
        /// Constructs a new EnumConvertionAttribute with specified convertion options
        /// </summary>
        /// <param name="convertionInfo">EnumConvertionInfo controls the format of Enum converted</param>
        /// <exception cref="ArgumentNullException">throw if <paramref name="convertionInfo"/> is null</exception>
        public EnumConvertionAttribute(EnumConvertionInfo convertionInfo)
        {
            ConvertionInfo = convertionInfo ?? throw new ArgumentNullException(nameof(convertionInfo));
        }
    }

    internal class EnumConvertionAttributeParser : IMemberAttributeParser
    {
        public void Parse(FieldInfo fieldInfo, TypeMetadata typeMetadata, MemberMetadata memberMetadata)
        {
            Parse(fieldInfo.GetCustomAttribute<EnumConvertionAttribute>(), typeMetadata, memberMetadata);
        }

        public void Parse(PropertyInfo propertyInfo, TypeMetadata typeMetadata, MemberMetadata memberMetadata)
        {
            Parse(propertyInfo.GetCustomAttribute<EnumConvertionAttribute>(), typeMetadata, memberMetadata);
        }

        private void Parse(EnumConvertionAttribute attribute, TypeMetadata typeMetadata, MemberMetadata memberMetadata)
        {
            if (attribute != null)
            {
                if ((memberMetadata.Attributes & MemberAttributes.Invalid) == MemberAttributes.Invalid)
                    throw new OhmSharpInvalidSchemaException(typeMetadata.Type, memberMetadata.Name,
                        string.Format("Member {0} of {1} cannot be marked with EnumConvertion.", memberMetadata.Name, typeMetadata.Type.FullName));
                if ((memberMetadata.Attributes & MemberAttributes.Ignored) == MemberAttributes.Ignored)
                    throw new OhmSharpInvalidSchemaException(typeMetadata.Type, memberMetadata.Name,
                        string.Format("Member {0} of {1} cannot be marked with both EnumConvertion and MappingIgnore.", memberMetadata.Name, typeMetadata.Type.FullName));

                if (!memberMetadata.Type.GetTypeInfo().IsEnum)
                    throw new OhmSharpInvalidSchemaException(typeMetadata.Type,
                        string.Format("Member {0} of {1} not of type Enum cannot be marked with EnumConvertion.", memberMetadata.Name, typeMetadata.Type.FullName));

                memberMetadata.Attributes |= MemberAttributes.Mapped;
                memberMetadata.FormatProvider = attribute.ConvertionInfo;
            }
        }
    }
}