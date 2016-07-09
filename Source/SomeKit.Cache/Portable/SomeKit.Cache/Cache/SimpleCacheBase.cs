using System;
using System.Collections.Generic;
using System.Linq;

namespace SomeKit.Cache.Cache
{
    /// <summary>
    /// Implements <see cref="ICache{T}"/>. Implemented by utilizing a single container for the entire cache.
    /// </summary>
    /// <typeparam name="T">The type of element in the records in the cache</typeparam>
    /// <typeparam name="CONTAINER">The type of container to hold the individual records</typeparam>
    public abstract class SimpleCacheBase<T, CONTAINER> : ICache<T>
        where T : IHasKey<int>
        where CONTAINER : IContainer<Record<T>>, new()
    {
        private readonly bool _shouldLock;
        private readonly CONTAINER _container = new CONTAINER();
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shouldLock">Flag telling whether to lock or not on each access</param>
        protected SimpleCacheBase(bool shouldLock)
        {
            _shouldLock = shouldLock;
        }

        #region Simple CRUD
        ///<inheritdoc/>
        public IQueryable<T> GetAll()
        {
            Func<IQueryable<T>> query = () =>
                _container.Select(x => x.Item).ToArray().AsQueryable();

            if (_shouldLock)
            {
                try
                {
                    StartLock();
                    return query();
                }
                finally
                {
                    EndLock();
                }
            }
            else return query();
        }
        ///<inheritdoc/>
        public bool TryGet(int key, out T value)
        {
            if (_shouldLock)
            {
                try
                {
                    StartLock();
                    Record<T> existingRecord;
                    value = default(T);
                    bool hasItem = _container.TryGet(key, out existingRecord);
                    if (hasItem)
                        value = existingRecord.Item;
                    return hasItem;
                }
                finally
                {
                    EndLock();
                }
            }
            else
            {
                Record<T> existingRecord;
                value = default(T);
                bool hasItem = _container.TryGet(key, out existingRecord);
                if (hasItem)
                    value = existingRecord.Item;
                return hasItem;
            }
        }
        ///<inheritdoc/>
        public T Get(int key)
        {
            Func<T> query = () =>
            {
                Record<T> existingRecord;
                if (!_container.TryGet(key, out existingRecord))
                    throw new KeyNotFoundException();
                return existingRecord.Item;
            };

            if (_shouldLock)
            {
                try
                {
                    StartLock();
                    return query();
                }
                finally
                {
                    EndLock();
                }
            }
            else return query();
        }
        ///<inheritdoc/>
        public void Upsert(T value)
        {
            Action<T> action = x =>
            {
                Record<T> existingRecord;
                bool hasItem = _container.TryGet(value.Key, out existingRecord);
                if (hasItem)
                    existingRecord.Item = value;
                else _container.Set(value.Key, new Record<T>(value));
            };

            if (_shouldLock)
            {
                try
                {
                    StartLock();
                    action(value);
                }
                finally
                {
                    EndLock();
                }
            }
            else action(value);
        }
        ///<inheritdoc/>
        public void Delete(int key)
        {
            Action<int> action = x =>
            {
                bool hadItem = _container.TryRemove(key);
                if (!hadItem)
                    throw new KeyNotFoundException();
            };

            if (_shouldLock)
            {
                try
                {
                    StartLock();
                    action(key);
                }
                finally
                {
                    EndLock();
                }
            }
            else action(key);
        }
        ///<inheritdoc/>
        public void Clear()
        {
            Action action = () => _container.Clear();

            if (_shouldLock)
            {
                try
                {
                    StartLock();
                    action();
                }
                finally
                {
                    EndLock();
                }
            }
            else action();
        }
        #endregion

        #region Advanced CRUD
        ///<inheritdoc/>
        public IQueryable<T> Query(Predicate<T> filter)
        {
            Func<IQueryable<T>> query = () =>
                _container.Where(x => filter(x.Item)).Select(x => x.Item).ToArray().AsQueryable();

            if (_shouldLock)
            {
                try
                {
                    StartLock();
                    return query();
                }
                finally
                {
                    EndLock();
                }
            }
            else return query();
        }
        ///<inheritdoc/>
        public IQueryable<U> Query<U>(Predicate<T> filter, Func<T, U> transform)
        {
            Func<IQueryable<U>> query = () =>
                _container.Where(x => filter(x.Item)).Select(x => transform(x.Item)).ToArray().AsQueryable();

            if (_shouldLock)
            {
                try
                {
                    StartLock();
                    return query();
                }
                finally
                {
                    EndLock();
                }
            }
            else return query();
        }
        ///<inheritdoc/>
        public void BulkUpsert(IEnumerable<T> values)
        {
            Action<IEnumerable<T>> action = x =>
            {
                foreach (var value in values)
                {
                    Record<T> existingRecord;
                    bool hasItem = _container.TryGet(value.Key, out existingRecord);
                    if (hasItem)
                        existingRecord.Item = value;
                    else _container.Set(value.Key, new Record<T>(value));
                }
            };

            if (_shouldLock)
            {
                try
                {
                    StartLock();
                    action(values);
                }
                finally
                {
                    EndLock();
                }
            }
            else action(values);
        }
        ///<inheritdoc/>
        public void BulkDelete(IEnumerable<int> keys)
        {
            Action<IEnumerable<int>> action = x =>
            {
                if (keys.Select(key => _container.TryRemove(key)).Any(hadItem => !hadItem))
                {
                    throw new KeyNotFoundException();
                }
            };

            if (_shouldLock)
            {
                try
                {
                    StartLock();
                    action(keys);
                }
                finally
                {
                    EndLock();
                }
            }
            else action(keys);
        }
        ///<inheritdoc/>
        public void Modify(Action<T> handler, Predicate<T> filter = null)
        {
            Action action;
            if (filter == null)
                action = () =>
                {
                    foreach (var record in _container)
                        handler(record.Item);
                };
            else
                action = () =>
                {
                    foreach (var record in _container.Where(x => filter(x.Item)))
                        handler(record.Item);
                };

            if (_shouldLock)
            {
                try
                {
                    StartLock();
                    action();
                }
                finally
                {
                    EndLock();
                }
            }
            else action();
        }
        #endregion
        /// <summary>
        /// Virtual (default empty) method utlized to provide locking.
        /// </summary>
        protected virtual void StartLock()
        {
        }
        /// <summary>
        /// Virtual (default empty) method utilized to provide lock release
        /// </summary>
        protected virtual void EndLock()
        {
        }
    }
}
