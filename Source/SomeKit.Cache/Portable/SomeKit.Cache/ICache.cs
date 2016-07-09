using System;
using System.Collections.Generic;
using System.Linq;

namespace SomeKit.Cache
{
    /// <summary>
    /// Interface describing a cache of records (<see cref="Record{T}"/>) or <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">The type of elements cached</typeparam>
    public interface ICache<T>
        where T : IHasKey<int>
    {
        #region Simple CRUD
        /// <summary>
        /// Fetches all elements present in the cache
        /// </summary>
        /// <returns>All elements in the cache</returns>
        IQueryable<T> GetAll();
        /// <summary>
        /// Attempts to fetch a given element at a given key
        /// </summary>
        /// <param name="key">The key of the element to get</param>
        /// <param name="value">The value to get</param>
        /// <returns>If element successfully fetched: True, else False</returns>
        bool TryGet(int key, out T value);
        /// <summary>
        /// Gets the element at a given key
        /// </summary>
        /// <param name="key">The key of the element</param>
        /// <returns>The element representing the key <paramref name="key"/></returns>
        T Get(int key);
        /// <summary>
        /// Upserts the element <paramref name="value"/>
        /// </summary>
        /// <param name="value">The value to upsert</param>
        void Upsert(T value);
        /// <summary>
        /// Deletes the value at a given key
        /// </summary>
        /// <param name="key">The key of the element to delete</param>
        void Delete(int key);
        /// <summary>
        /// Clears the cache
        /// </summary>
        void Clear();

        #endregion

        #region Advanced CRUD
        /// <summary>
        /// Fetches all elements satisfying a given <paramref name="filter"/>
        /// </summary>
        /// <param name="filter">The filter representing the query</param>
        /// <returns>All alements in the cache satisfying the query</returns>
        IQueryable<T> Query(Predicate<T> filter);
        /// <summary>
        /// Fetches a view of elements satisfying a given <paramref name="filter"/>
        /// </summary>
        /// <typeparam name="U">Type of the view</typeparam>
        /// <param name="filter">The filter representing the query</param>
        /// <param name="transform">A transform</param>
        /// <returns></returns>
        IQueryable<U> Query<U>(Predicate<T> filter, Func<T, U> transform);
        /// <summary>
        /// Bulk upserts a sequence of elements
        /// </summary>
        /// <param name="values">The elements to upsert</param>
        void BulkUpsert(IEnumerable<T> values);
        /// <summary>
        /// Bult deletes a sequence of elements
        /// </summary>
        /// <param name="keys">The keys representing the elements to delete</param>
        void BulkDelete(IEnumerable<int> keys);
        /// <summary>
        /// Executes a handling on each element in  the cache (satisfying to optional filter if provided)
        /// </summary>
        /// <param name="handler">The action performed on each (matching) element</param>
        /// <param name="filter">The filter (optional)</param>
        void Modify(Action<T> handler, Predicate<T> filter = null);

        #endregion
    }
}
