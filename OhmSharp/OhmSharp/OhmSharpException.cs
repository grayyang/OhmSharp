using System;

namespace OhmSharp
{
    /// <summary>
    /// Base Exception type for exception in OhmSharp
    /// </summary>
    public class OhmSharpException : Exception
    {
        internal OhmSharpException(string message, Exception innerException)
            : base(message, innerException)
        { }

        internal OhmSharpException(string message)
            : base(message)
        { }
    }
}
