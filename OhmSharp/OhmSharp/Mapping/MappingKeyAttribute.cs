using System;
using System.Reflection;

namespace OhmSharp.Mapping
{
    /// <summary>
    /// Specifies that value of this property or field is used as identity part of Redis key of the object
    /// Only one property or field can be annotated as MappingKey
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class MappingKeyAttribute : Attribute
    {
        /// <summary>
        /// Constructs a new MappingKeyAttribute attribute
        /// </summary>
        public MappingKeyAttribute()
        { }
    }

    internal class MappingKeyAttributeParser : IMemberAttributeParser
    {
        public void Parse(FieldInfo fieldInfo, TypeMetadata typeMetadata, MemberMetadata memberMetadata)
        {
            Parse(fieldInfo.GetCustomAttribute<MappingKeyAttribute>(), typeMetadata, memberMetadata);
        }

        public void Parse(PropertyInfo propertyInfo, TypeMetadata typeMetadata, MemberMetadata memberMetadata)
        {
            Parse(propertyInfo.GetCustomAttribute<MappingKeyAttribute>(), typeMetadata, memberMetadata);
        }

        private void Parse(MappingKeyAttribute attribute, TypeMetadata typeMetadata, MemberMetadata memberMetadata)
        {
            if (attribute != null)
            {
                if ((memberMetadata.Attributes & MemberAttributes.Invalid) == MemberAttributes.Invalid)
                    throw new OhmSharpInvalidSchemaException(typeMetadata.Type, memberMetadata.Name,
                        string.Format("Member {0} of {1} cannot be marked with MappingKey.", memberMetadata.Name, typeMetadata.Type.FullName));
                if ((memberMetadata.Attributes & MemberAttributes.Ignored) == MemberAttributes.Ignored)
                    throw new OhmSharpInvalidSchemaException(typeMetadata.Type, memberMetadata.Name,
                        string.Format("Member {0} of {1} cannot be marked with both MappingKey and MappingIgnore.", memberMetadata.Name, typeMetadata.Type.FullName));

                // TODO: check for key type 

                if (typeMetadata.IdentityMember != null)
                    throw new OhmSharpInvalidSchemaException(typeMetadata.Type,
                        string.Format("Type {0} cannot contain more than one member marked with MappingKey.", typeMetadata.Type.FullName)); 

                memberMetadata.Attributes |= MemberAttributes.Mapped;
                typeMetadata.IdentityMember = memberMetadata;
            }
        }
    }
}
