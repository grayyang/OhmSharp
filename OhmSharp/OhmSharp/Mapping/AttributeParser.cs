using System;
using System.Reflection;

namespace OhmSharp.Mapping
{
    internal abstract class TypeAttributeParser<TAttribute> : ITypeParser where TAttribute : Attribute
    {
        public void Parse(TypeInfo typeInfo, TypeMetadata typeMetadata)
        {
            Parse(typeInfo.GetCustomAttribute<TAttribute>(), typeMetadata);
        }

        protected abstract void Parse(TAttribute attribute, TypeMetadata typeMetadata);
    }

    internal abstract class MemberAttributeParser<TAttribute> : IMemberParser where TAttribute : Attribute
    {
        public void Parse(FieldInfo fieldInfo, TypeMetadata typeMetadata, MemberMetadata memberMetadata)
        {
            Parse(fieldInfo.GetCustomAttribute<TAttribute>(), typeMetadata, memberMetadata);
        }

        public void Parse(PropertyInfo propertyInfo, TypeMetadata typeMetadata, MemberMetadata memberMetadata)
        {
            Parse(propertyInfo.GetCustomAttribute<TAttribute>(), typeMetadata, memberMetadata);
        }

        protected abstract void Parse(TAttribute attribute, TypeMetadata typeMetadata, MemberMetadata memberMetadata);
    }
}
