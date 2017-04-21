using System;
using System.Collections.Generic;
using System.Text;

namespace OhmSharp.Convertion
{
    /// <summary>
    /// Exception indicate convertion failed from or to RedisValue
    /// </summary>
    public class OhmSharpConvertionException : OhmSharpException
    {
        /// <summary>
        /// Type failed to convert from or to RedisValue
        /// </summary>
        public Type Type { get; private set; }

        internal OhmSharpConvertionException(Type type, string message)
            : base(message)
        {
            Type = type;
        }

        internal OhmSharpConvertionException(Type type, string message, Exception innerException)
            : base(message, innerException)
        {
            Type = type;
        }
    }
}
