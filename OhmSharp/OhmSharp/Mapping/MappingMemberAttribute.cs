using System;

namespace OhmSharp.Mapping
{
    /// <summary>
    /// Specifies that the this property or field should be mapped to Redis store
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class MappingMemberAttribute : Attribute
    {
        /// <summary>
        /// Name used as key to save the property or field value in Redis
        /// </summary>
        public string Name { get; set; } = null;

        /// <summary>
        /// Constructs a new MappingMemberAttribute attribute
        /// </summary>
        public MappingMemberAttribute()
        { }

        /// <summary>
        /// Constructs a new MappingMemberAttribute attribute with member name specified
        /// </summary>
        /// <param name="name">name used as key to save the property or field</param>
        public MappingMemberAttribute(string name)
        {
            Name = name;
        }
    }

    internal class MappingMemberAttributeParser : MemberAttributeParser<MappingMemberAttribute>
    {
        protected override void Parse(MappingMemberAttribute attribute, TypeMetadata typeMetadata, MemberMetadata memberMetadata)
        {
            if (attribute != null)
            {
                if ((memberMetadata.Attributes & MemberAttributes.Unmappable) == MemberAttributes.Unmappable)
                    throw new OhmSharpInvalidSchemaException(typeMetadata.Type, memberMetadata.Name,
                        string.Format("Member {0} of {1} cannot be marked with MappingMember.", memberMetadata.Name, typeMetadata.Type.FullName));
                if ((memberMetadata.Attributes & MemberAttributes.Ignored) == MemberAttributes.Ignored)
                    throw new OhmSharpInvalidSchemaException(typeMetadata.Type, memberMetadata.Name,
                        string.Format("Member {0} of {1} cannot be marked with both MappingMember and MappingIgnore.", memberMetadata.Name, typeMetadata.Type.FullName));

                memberMetadata.Attributes |= MemberAttributes.Mapped;
                memberMetadata.MemberName = attribute.Name ?? memberMetadata.Name;
            }
        }
    }
}
