using System;
#pragma warning disable 0108

namespace OhmSharp.Mapper.Identity
{
    /// <summary>
    /// Defines methods that uses object of type <typeparamref name="T"/> as identity part of RedisKey
    /// </summary>
    /// <typeparam name="T">type of object</typeparam>
    public interface IObjectIdentity<T> : IObjectIdentity
    {
        /// <summary>
        /// Parses identity from string form to its underlying type 
        /// </summary>
        /// <param name="identity">identity in string form</param>
        /// <returns>identity parsed from its string form</returns>
        T Parse(string identity);

        /// <summary>
        /// Converts identity to string form
        /// </summary>
        /// <param name="identity">identity to convert</param>
        /// <returns>string form converted from the identity</returns>
        string ToString(T identity);

        /// <summary>
        /// Whether or not the identity is an invalid empty value
        /// </summary>
        /// <param name="identity">identity to check</param>
        /// <returns>true if identity is empty; otherwise, false</returns>
        bool IsEmpty(T identity);

        /// <summary>
        /// Generates one new unique identity
        /// </summary>
        /// <returns>new unique identity</returns>
        T GenerateNew();
    }

    /// <summary>
    /// Defines methods that uses object as identity part of RedisKey
    /// </summary>
    public interface IObjectIdentity
    {
        /// <summary>
        /// Parses identity from string form to its underlying type 
        /// </summary>
        /// <param name="identity">identity in string form</param>
        /// <returns>identity parsed from its string form</returns>
        object Parse(string identity);

        /// <summary>
        /// Converts identity to string form
        /// </summary>
        /// <param name="identity">identity to convert</param>
        /// <returns>string form converted from the identity</returns>
        string ToString(object identity);

        /// <summary>
        /// Whether or not the identity is an invalid empty value
        /// </summary>
        /// <param name="identity">identity to check</param>
        /// <returns>true if identity is empty; otherwise, false</returns>
        bool IsEmpty(object identity);

        /// <summary>
        /// Whether or not this <see cref="IObjectIdentity"/> supports generate new unique identity
        /// </summary>
        bool CanGenerate { get; }

        /// <summary>
        /// Generates one new unique identity
        /// </summary>
        /// <returns>new unique identity</returns>
        object GenerateNew();
    }
}
