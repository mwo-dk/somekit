using System.Collections;
using System.Collections.Generic;

namespace SomeKit.Cache.Container
{
    /// <summary>
    /// Base implementation of <see cref="IContainer{T}"/>
    /// </summary>
    /// <typeparam name="T">The type of element in the container</typeparam>
    public abstract class ContainerBase<T> :
        IContainer<T>
    {
        ///<inheritdoc/>
        public abstract int Size { get; }
        ///<inheritdoc/>
        public abstract bool TryGet(int position, out T value);
        ///<inheritdoc/>
        public abstract void Set(int position, T value);
        ///<inheritdoc/>
        public abstract bool TryRemove(int position);
        ///<inheritdoc/>
        public abstract void Clear();
        /// <summary>
        /// The enumerator
        /// </summary>
        /// <returns>An enumerator</returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (var n = 0; n < Size; ++n)
            {
                T value;
                TryGet(n, out value);
                yield return value;
            }
        }
        /// <summary>
        /// The enumerator
        /// </summary>
        /// <returns>An enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
