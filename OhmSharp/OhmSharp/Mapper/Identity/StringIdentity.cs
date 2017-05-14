using System;

namespace OhmSharp.Mapper.Identity
{
    internal class StringIdentity : IObjectIdentity<string>
    {
        public bool CanGenerate => false;

        public string GenerateNew()
        {
            throw new NotSupportedException("StringIdentityGenerator does not support generate new unique identity.");
        }

        public bool IsEmpty(string identity)
        {
            return string.IsNullOrWhiteSpace(identity);
        }

        public string Parse(string identity)
        {
            return identity;
        }

        public string ToString(string identity)
        {
            return identity;
        }

        public bool IsEmpty(object identity)
        {
            return this.IsEmpty(identity);
        }

        public string ToString(object identity)
        {
            return this.ToString(identity);
        }

        object IObjectIdentity.GenerateNew()
        {
            return this.GenerateNew();
        }

        object IObjectIdentity.Parse(string identity)
        {
            return this.Parse(identity);
        }
    }
}
