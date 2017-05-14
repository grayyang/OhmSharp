using System;

namespace OhmSharp.Mapper.Identity
{
    internal class GuidIdentity : IObjectIdentity<Guid>
    {
        public bool CanGenerate => true;

        public Guid GenerateNew()
        {
            return Guid.NewGuid();
        }

        public bool IsEmpty(Guid identity)
        {
            return Guid.Empty == identity;
        }

        public Guid Parse(string identity)
        {
            return Guid.Parse(identity);
        }

        public string ToString(Guid identity)
        {
            return identity.ToString();
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
