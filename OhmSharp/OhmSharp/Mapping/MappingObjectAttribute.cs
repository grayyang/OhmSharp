using System;

namespace OhmSharp.Mapping
{
    /// <summary>
    /// Specifies that this type should be mapped to Redis store
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class MappingObjectAttribute : Attribute
    {
        /// <summary>
        /// Name used as Redis key string for this type of object in Redis
        /// Note that this may need to be override to make object evenly distributed if Redis Cluster is used
        /// </summary>
        public string Name { get; set; } = null;

        /// <summary>
        /// Whether optimistic concurrency is enabled for the type or not
        /// Default value is false
        /// </summary>
        public bool Concurrency { get; set; } = false;

        /// <summary>
        /// Constructs a new MappingObjectAttribute attribute
        /// </summary>
        public MappingObjectAttribute()
        { }

        /// <summary>
        /// Constructs a new MappingObjectAttribute attribute with key name specified
        /// </summary>
        /// <param name="name">name used as key to save object</param>
        public MappingObjectAttribute(string name)
        {
            Name = name;
        }
    }

    internal class MappingObjectAttributeParser : TypeAttributeParser<MappingObjectAttribute>
    {
        protected override void Parse(MappingObjectAttribute attribute, TypeMetadata typeMetadata)
        {
            if (attribute != null)
            {
                typeMetadata.KeyName = attribute.Name ?? typeMetadata.KeyName;
                typeMetadata.Concurrency = attribute.Concurrency;
            }
        }
    }
}
