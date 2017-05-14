using System;
using System.Collections.Generic;
using System.Text;

namespace OhmSharp.Mapping.Schema
{
    public class ObjectSchema
    {
        internal ObjectSchema(TypeMetadata metadata)
        {
            IsConcurrencyEnabled = metadata.Concurrency || metadata.ConcurrencyMember != null;
        }

        /// <summary>
        /// Whether optimistic concurrency checking is enabled or not
        /// </summary>
        public bool IsConcurrencyEnabled { get; private set; }
    }
}
