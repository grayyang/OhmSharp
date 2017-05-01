using System;
using System.Reflection;

namespace OhmSharp.Mapping
{
    /// <summary>
    /// Specifies that the value of this property or field should be indexed for better query performance
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class MappingIndexAttribute : Attribute
    {
        /// <summary>
        /// Constructs a new MappingIndexAttribute attribute
        /// </summary>
        public MappingIndexAttribute()
        { }
    }

    internal class MappingIndexAttributeParser : IMemberAttributeParser
    {
        public void Parse(FieldInfo fieldInfo, TypeMetadata typeMetadata, MemberMetadata memberMetadata)
        {
            Parse(fieldInfo.GetCustomAttribute<MappingIndexAttribute>(), typeMetadata, memberMetadata);
        }

        public void Parse(PropertyInfo propertyInfo, TypeMetadata typeMetadata, MemberMetadata memberMetadata)
        {
            Parse(propertyInfo.GetCustomAttribute<MappingIndexAttribute>(), typeMetadata, memberMetadata);
        }

        private void Parse(MappingIndexAttribute attribute, TypeMetadata typeMetadata, MemberMetadata memberMetadata)
        {
            if (attribute != null)
            {
                if ((memberMetadata.Attributes & MemberAttributes.Invalid) == MemberAttributes.Invalid)
                    throw new OhmSharpInvalidSchemaException(typeMetadata.Type, memberMetadata.Name,
                        string.Format("Member {0} of {1} cannot be marked with MappingIndex.", memberMetadata.Name, typeMetadata.Type.FullName));
                if ((memberMetadata.Attributes & MemberAttributes.Ignored) == MemberAttributes.Ignored)
                    throw new OhmSharpInvalidSchemaException(typeMetadata.Type, memberMetadata.Name,
                        string.Format("Member {0} of {1} cannot be marked with both MappingIndex and MappingIgnore.", memberMetadata.Name, typeMetadata.Type.FullName));

                // TODO: check for index type 

                memberMetadata.Attributes |= MemberAttributes.Mapped;
                typeMetadata.IndexedMembers.Add(memberMetadata);
            }
        }
    }
}
