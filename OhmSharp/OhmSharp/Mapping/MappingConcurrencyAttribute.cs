using OhmSharp.Convertion;
using System;

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

    internal class MappingConcurrencyAttributeParser : MemberAttributeParser<MappingConcurrencyAttribute>
    {
        protected override void Parse(MappingConcurrencyAttribute attribute, TypeMetadata typeMetadata, MemberMetadata memberMetadata)
        {
            if (attribute != null)
            {
                if ((memberMetadata.Attributes & MemberAttributes.Unmappable) == MemberAttributes.Unmappable)
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

                if (memberMetadata.FormatProvider != null && ((DateTimeConvertionInfo)memberMetadata.FormatProvider).DateOnly)
                    throw new OhmSharpInvalidSchemaException(typeMetadata.Type, memberMetadata.Name,
                        string.Format("Member {0} of {1} marked with MappingConcurrency cannot use DateTimeConvertionInfo.AsDate.", memberMetadata.Name, typeMetadata.Type.FullName));

                memberMetadata.Attributes |= MemberAttributes.Mapped;
                typeMetadata.ConcurrencyMember = memberMetadata;
            }
        }
    }
}
