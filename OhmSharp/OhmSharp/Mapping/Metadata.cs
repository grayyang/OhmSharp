using System;
using System.Collections.Generic;

namespace OhmSharp.Mapping
{
    internal class TypeMetadata
    {
        public Type Type;
        public string KeyName;
        public bool Concurrency;

        public MemberMetadata IdentityMember;
        public List<MemberMetadata> TypeMembers;
        public List<MemberMetadata> IndexedMembers;
        public MemberMetadata ConcurrencyMember;

        public TypeMetadata(Type type, string name)
        {
            Type = type;
            KeyName = name;

            Concurrency = false;
            IdentityMember = null;
            ConcurrencyMember = null;
            TypeMembers = new List<MemberMetadata>();
            IndexedMembers = new List<MemberMetadata>();
        }
    }

    internal class MemberMetadata
    {
        public Type Type;
        public string Name;

        public MemberAttributes Attributes;
        public string MemberName;
        public GetterSetterAttributes Getter;
        public GetterSetterAttributes Setter;
        public IFormatProvider FormatProvider;

        public MemberMetadata(Type type, string name)
        {
            Type = type;
            Name = name;
            MemberName = name;

            Attributes = MemberAttributes.None;
            Getter = GetterSetterAttributes.None;
            Setter = GetterSetterAttributes.None;
            FormatProvider = null;
        }
    }

    /// <summary>
    /// Attributes of type member
    /// </summary>
    [Flags]
    internal enum MemberAttributes : uint
    {
        /// <summary>
        /// default for member not parsed yet
        /// </summary>
        None = 0,
        /// <summary>
        /// member is marked as mapped
        /// </summary>
        Mapped = 0x1,
        /// <summary>
        /// member is marked as ignored
        /// </summary>
        Ignored = 0x2,
        /// <summary>
        /// member is not able to be mapped (static or private)
        /// </summary>
        Unmappable = 0x4,
        /// <summary>
        /// member is a property
        /// </summary>
        Property = 0x10,
        /// <summary>
        /// member is a field
        /// </summary>
        Field = 0x20,
    }

    /// <summary>
    /// Attributes of getter or setter
    /// </summary>
    [Flags]
    internal enum GetterSetterAttributes : uint
    {
        /// <summary>
        /// default for getter/setter not parsed yet
        /// </summary>
        None = 0,
        /// <summary>
        /// getter/setter is defined
        /// </summary>
        Defined = 0x1,
        /// <summary>
        /// getter/setter is virtual
        /// </summary>
        Virtual = 0x2,
    }
}
