﻿using System;
using System.Reflection;

namespace OhmSharp.Mapping
{
    /// <summary>
    /// Specifies that this property or field is used to save timestamp for concurrency checking
    /// Only one property or field can be annotated as MappingKey
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class MappingConcurrencyAttribute : Attribute
    {
        /// <summary>
        /// Constructs a new MappingConcurrencyAttribute attribute
        /// </summary>
        public MappingConcurrencyAttribute()
        { }
    }

    internal class MappingConcurrencyAttributeParser : IMemberAttributeParser
    {
        public void Parse(FieldInfo fieldInfo, TypeMetadata typeMetadata, MemberMetadata memberMetadata)
        {
            Parse(fieldInfo.GetCustomAttribute<MappingConcurrencyAttribute>(), typeMetadata, memberMetadata);
        }

        public void Parse(PropertyInfo propertyInfo, TypeMetadata typeMetadata, MemberMetadata memberMetadata)
        {
            Parse(propertyInfo.GetCustomAttribute<MappingConcurrencyAttribute>(), typeMetadata, memberMetadata);
        }

        private void Parse(MappingConcurrencyAttribute attribute, TypeMetadata typeMetadata, MemberMetadata memberMetadata)
        {
            if (attribute != null)
            {
                if ((memberMetadata.Attributes & MemberAttributes.Invalid) == MemberAttributes.Invalid)
                    throw new OhmSharpInvalidSchemaException(typeMetadata.Type, memberMetadata.Name,
                        string.Format("Member {0} of {1} cannot be marked with MappingConcurrency.", memberMetadata.Name, typeMetadata.Type.FullName));
                if ((memberMetadata.Attributes & MemberAttributes.Ignored) == MemberAttributes.Ignored)
                    throw new OhmSharpInvalidSchemaException(typeMetadata.Type, memberMetadata.Name,
                        string.Format("Member {0} of {1} cannot be marked with both MappingConcurrency and MappingIgnore.", memberMetadata.Name, typeMetadata.Type.FullName));

                if (memberMetadata.Type != typeof(DateTime))
                    throw new OhmSharpInvalidSchemaException(typeMetadata.Type, memberMetadata.Name,
                        string.Format("Member {0} of {1} not of type DateTime cannot be marked with MappingConcurrency.", memberMetadata.Name, typeMetadata.Type.FullName));

                if (typeMetadata.ConcurrencyMember != null)
                    throw new OhmSharpInvalidSchemaException(typeMetadata.Type,
                        string.Format("Type {0} cannot contain more than one member marked with MappingConcurrency.", typeMetadata.Type.FullName));

                memberMetadata.Attributes |= MemberAttributes.Mapped;
                typeMetadata.ConcurrencyMember = memberMetadata;
            }
        }
    }
}
