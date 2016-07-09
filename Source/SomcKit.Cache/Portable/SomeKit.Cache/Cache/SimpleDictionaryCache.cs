using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SomeKit.Cache.Container;

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
