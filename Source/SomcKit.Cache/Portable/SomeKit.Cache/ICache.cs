using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeKit.Cache
{
    public interface ICache<T>
        where T : IHasKey<int>
    {
        #region Simple CRUD
        IQueryable<T> GetAll();
        bool TryGet(int key, out T value);
        T Get(int key);
        void Upsert(T value);
        void Delete(int key);
        void Clear();

        #endregion

        #region Advanced CRUD

        IQueryable<T> Query(Predicate<T> filter);
        IQueryable<U> Query<U>(Predicate<T> filter, Func<T, U> transform);
        void BulkUpsert(IEnumerable<T> values);
        void BulkDelete(IEnumerable<int> keys);
        void Modify(Action<T> handler, Predicate<T> filter = null);

        #endregion
    }
}
