using System.Collections.Generic;

namespace SomeKit.Cache
{
    /// <summary>
    /// Interface describing a physical container of data
    /// </summary>
    /// <typeparam name="T">The type of element in the container</typeparam>
    public interface IContainer<T> :
        IEnumerable<T>
    {
        /// <summary>
        /// The physical size (in count of actual elements) that is reserved for elements
        /// </summary>
        int Size { get; }
        /// <summary>
        /// Attempts to fetch a an element at a given position
        /// </summary>
        /// <param name="position">The position of the given element</param>
        /// <param name="value">The value at the given position</param>
        /// <returns>If element is present: True, else False</returns>
        bool TryGet(int position, out T value);
        /// <summary>
        /// Sets a given value at a given position
        /// </summary>
        /// <param name="position">The position where to put the given value</param>
        /// <param name="value">The value to put</param>
        void Set(int position, T value);
        /// <summary>
        /// Attempts to remove an element at a given position
        /// </summary>
        /// <param name="position">The position of the element to remove</param>
        /// <returns>If element is successfully removed: True, else False</returns>
        bool TryRemove(int position);
        /// <summary>
        /// Clears the container
        /// </summary>
        void Clear();
    }
}
