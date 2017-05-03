using System;
using System.Reflection;

namespace OhmSharp.Mapping
{
    /// <summary>
    /// Specifies that this property or field should not be mapped to Redis store
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class MappingIgnoreAttribute : Attribute
    {
        /// <summary>
        /// Constructs a new MappingIgnoreAttribute attribute
        /// </summary>
        public MappingIgnoreAttribute()
        { }
    }

    internal class MappingIgnoreAttributeParser : MemberAttributeParser<MappingIgnoreAttribute>
    {
        protected override void Parse(MappingIgnoreAttribute attribute, TypeMetadata typeMetadata, MemberMetadata memberMetadata)
        {
            if (attribute != null)
            {
                if ((memberMetadata.Attributes & MemberAttributes.Mapped) == MemberAttributes.Mapped)
                    throw new OhmSharpInvalidSchemaException(typeMetadata.Type, memberMetadata.Name,
                        string.Format("Member {0} of {1} cannot be marked with both MappingMember and MappingIgnore.", memberMetadata.Name, typeMetadata.Type.FullName));

                memberMetadata.Attributes |= MemberAttributes.Ignored;
            }
        }
    }
}
