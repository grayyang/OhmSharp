﻿using System;

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

    internal class MappingIndexAttributeParser : MemberAttributeParser<MappingIndexAttribute>
    {
        protected override void Parse(MappingIndexAttribute attribute, TypeMetadata typeMetadata, MemberMetadata memberMetadata)
        {
            if (attribute != null)
            {
                if ((memberMetadata.Attributes & MemberAttributes.Unmappable) == MemberAttributes.Unmappable)
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
