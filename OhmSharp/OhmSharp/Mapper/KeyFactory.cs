using StackExchange.Redis;
using System;

namespace OhmSharp.Mapper
{
    /// <summary>
    /// Provides methods to construct RedisKey used in object mapping
    /// </summary>
    internal static class KeyFactory
    {
        internal static RedisKey ObjectKey(string type, string id)
        {
            return ObjectKeyTemplate.ReplaceTemplate(nameof(type), type)
                .ReplaceTemplate(nameof(id), id);
        }

        internal static RedisKey CollectionKey(string type)
        {
            return CollectionKeyTemplate.ReplaceTemplate(nameof(type), type);
        }

        internal static RedisKey IdentityKey(string type)
        {
            return IdentityKeyTemplate.ReplaceTemplate(nameof(type), type);
        }

        internal static RedisKey IndexKey(string type, string member)
        {
            return IndexKeyTemplate.ReplaceTemplate(nameof(type), type)
                .ReplaceTemplate(nameof(member), member);
        }

        internal static RedisKey IndexValueCollectionKey(string type, string member)
        {
            return IndexValueCollectionKeyTemplate.ReplaceTemplate(nameof(type), type)
                .ReplaceTemplate(nameof(member), member);
        }

        internal static RedisKey IndexValueCollectionKey(string type, string member, string value)
        {
            return IndexValueCollectionKeyTemplate.ReplaceTemplate(nameof(type), type)
                .ReplaceTemplate(nameof(member), member)
                .ReplaceTemplate(nameof(value), value);
        }

        internal static string ReplaceTemplate(this string key, string template, string value)
        {
            template = string.Format("<{0}>", template);
            if (key.IndexOf(template) == -1)
                throw new InvalidOperationException(string.Format("{0} is not template within key {1}", template, key));

            return key.Replace(template, value);
        }

        private static readonly string ObjectKeyTemplate = "{<type>}:<id>";

        private static readonly string CollectionKeyTemplate = "{<type>}:$collection$";

        private static readonly string IdentityKeyTemplate = "{<type>}:$identity$";

        private static readonly string IndexKeyTemplate = "{<type>}:$index$:<member>";

        private static readonly string IndexValueCollectionKeyTemplate = "{<type>}:$index$:<member>:<value>";
    }
}
