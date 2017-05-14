using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OhmSharp.Mapping
{
    internal interface ITypeParser
    {
        void Parse(TypeInfo typeInfo, TypeMetadata typeMetadata);
    }

    internal interface IMemberParser
    {
        void Parse(FieldInfo fieldInfo, TypeMetadata typeMetadata, MemberMetadata memberMetadata);
        void Parse(PropertyInfo propertyInfo, TypeMetadata typeMetadata, MemberMetadata memberMetadata);
    }

    internal static class MetadataParser
    {
        internal static TypeMetadata Parse(Type type)
        {
            var typeMetadate = new TypeMetadata(type, type.FullName);

            foreach (var parser in _typeParsers)
            {
                parser.Parse(type.GetTypeInfo(), typeMetadate);
            }

            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
            {
                var memberMetadate = new MemberMetadata(field.FieldType, field.Name);
                typeMetadate.TypeMembers.Add(memberMetadate);

                foreach (var parser in _memberParsers)
                {
                    parser.Parse(field, typeMetadate, memberMetadate);
                }
            }

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
            {
                var memberMetadate = new MemberMetadata(property.PropertyType, property.Name);
                typeMetadate.TypeMembers.Add(memberMetadate);

                foreach (var parser in _memberParsers)
                {
                    parser.Parse(property, typeMetadate, memberMetadate);
                }
            }

            return typeMetadate;
        }

        static MetadataParser()
        {
            _typeParsers = new List<ITypeParser>
            {
                new BasicTypeParser(),
                new MappingObjectAttributeParser(),
            };

            _memberParsers = new List<IMemberParser>
            {
                new BasicMemberParser(),
                new MappingIgnoreAttributeParser(),
                new MappingMemberAttributeParser(),
                new MappingKeyAttributeParser(),
                new MappingConcurrencyAttributeParser(),
                new MappingIndexAttributeParser(),
                new EnumConvertionAttributeParser(),
                new DateTimeConvertionAttributeParser(),
            };
        }

        private static List<ITypeParser> _typeParsers;
        private static List<IMemberParser> _memberParsers;
    }

    internal class BasicTypeParser : ITypeParser
    {
        public void Parse(TypeInfo typeInfo, TypeMetadata typeMetadata)
        {
            if (typeInfo.IsArray || typeInfo.IsEnum || typeInfo.IsInterface ||
                typeInfo.IsPointer || typeInfo.IsPrimitive || typeInfo.IsSpecialName)
                throw new OhmSharpInvalidSchemaException(typeInfo.AsType(),
                    string.Format("Type {0} is of invalid type as mapping object.", typeInfo.FullName));

            if (!typeInfo.IsVisible)
                throw new OhmSharpInvalidSchemaException(typeInfo.AsType(),
                    string.Format("Type {0} is not public.", typeInfo.FullName));

            if (typeInfo.IsSealed)
                throw new OhmSharpInvalidSchemaException(typeInfo.AsType(),
                    string.Format("Type {0} is sealed.", typeInfo.FullName));

            var ctors = typeInfo.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (ctors.All(ctor => (!ctor.IsPublic && !ctor.IsFamily) || ctor.GetParameters().Length != 0))
                throw new OhmSharpInvalidSchemaException(typeInfo.AsType(),
                    string.Format("Type {0} has no public or protected parameterless constructor.", typeInfo.FullName));
        }
    }

    internal class BasicMemberParser : IMemberParser
    {
        public void Parse(FieldInfo fieldInfo, TypeMetadata typeMetadata, MemberMetadata memberMetadata)
        {
            memberMetadata.Attributes = MemberAttributes.Field;

            if ((!fieldInfo.IsPublic && !fieldInfo.IsFamily) || fieldInfo.IsStatic ||
                fieldInfo.IsSpecialName || fieldInfo.IsLiteral || fieldInfo.IsInitOnly)
                memberMetadata.Attributes |= MemberAttributes.Unmappable;

            memberMetadata.Getter = GetterSetterAttributes.Defined;
            memberMetadata.Setter = GetterSetterAttributes.Defined;
        }

        public void Parse(PropertyInfo propertyInfo, TypeMetadata typeMetadata, MemberMetadata memberMetadata)
        {
            memberMetadata.Attributes = MemberAttributes.Property;

            if (!propertyInfo.CanRead || (!propertyInfo.GetMethod.IsPublic && !propertyInfo.GetMethod.IsFamily) ||
                propertyInfo.GetMethod.IsStatic || propertyInfo.GetIndexParameters().Length > 0)
                memberMetadata.Attributes |= MemberAttributes.Unmappable;

            if (propertyInfo.CanRead)
            {
                if ((propertyInfo.GetMethod.IsPublic || propertyInfo.GetMethod.IsFamily) && !propertyInfo.GetMethod.IsStatic)
                    memberMetadata.Getter = GetterSetterAttributes.Defined;

                if (propertyInfo.GetMethod.IsVirtual && !propertyInfo.GetMethod.IsFinal)
                    memberMetadata.Getter |= GetterSetterAttributes.Virtual;
            }

            if (propertyInfo.CanWrite)
            {
                if ((propertyInfo.SetMethod.IsPublic || propertyInfo.SetMethod.IsFamily) && !propertyInfo.SetMethod.IsStatic)
                    memberMetadata.Setter = GetterSetterAttributes.Defined;

                if (propertyInfo.SetMethod.IsVirtual && !propertyInfo.SetMethod.IsFinal)
                    memberMetadata.Setter |= GetterSetterAttributes.Virtual;
            }
        }
    }
}
