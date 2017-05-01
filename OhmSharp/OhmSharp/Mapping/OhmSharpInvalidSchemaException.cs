using System;

namespace OhmSharp.Mapping
{
    /// <summary>
    /// Exception indicates that the schema specified by mapping attributes is not valid
    /// </summary>
    public class OhmSharpInvalidSchemaException : OhmSharpException
    {
        /// <summary>
        /// Type causes invalid schema
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// Name of field or property causes invalid schema
        /// </summary>
        public string Member { get; private set; }

        internal OhmSharpInvalidSchemaException(Type type, string message)
            : this(type, null, message)
        { }

        internal OhmSharpInvalidSchemaException(Type type, string member, string message)
            : base(message)
        {
            Type = type;
            Member = member;
        }
    }
}
