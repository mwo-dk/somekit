﻿using SomeKit.Cache.Container;
using System.Threading;

namespace SomeKit.Cache.Cache
{
    public sealed class SimpleArrayCache<T> : SimpleCacheBase<T, ArrayContainer<Record<T>>>
        where T : IHasKey<int>
    {
        private readonly object _lock = new object();

        public SimpleArrayCache() : base(true)
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