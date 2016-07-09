using SomeKit.Cache.Container;
using System.Threading;

namespace SomeKit.Cache.Cache
{
    public sealed class SimpleDictionaryCache<T> : SimpleCacheBase<T, DictionaryContainer<Record<T>>>
        where T : IHasKey<int>
    {
        private readonly object _lock = new object();

        public SimpleDictionaryCache() : base(true)
        {
        }

        protected override void StartLock()
        {
            Monitor.Enter(_lock);
        }

        protected override void EndLock()
        {
            Monitor.Exit(_lock);
        }
    }
}
