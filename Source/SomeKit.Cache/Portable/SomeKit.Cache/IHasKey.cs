using System;

namespace SomeKit.Cache
{
    public interface IHasKey<out T>
        where T : IComparable<T>, IEquatable<T>
    {
        T Key { get; }
    }
}
