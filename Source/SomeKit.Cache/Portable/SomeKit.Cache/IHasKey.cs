using System;

namespace SomeKit.Cache
{
    /// <summary>
    /// Interface representing an "indexable" item
    /// </summary>
    /// <typeparam name="T">The type of key</typeparam>
    public interface IHasKey<out T>
        where T : IComparable<T>, IEquatable<T>
    {
        /// <summary>
        /// The key
        /// </summary>
        T Key { get; }
    }
}
