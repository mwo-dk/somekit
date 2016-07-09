using SomeKit.Cache.Container;
using System.Threading;

namespace SomeKit.Cache.Cache
{
    /// <summary>
    /// <see cref="SimpleCacheBase{T,CONTAINER}"/> implemented via the <see cref="ArrayContainer{T}"/>
    /// </summary>
    /// <typeparam name="T">The type of elements in each record in the container</typeparam>
    public sealed class SimpleArrayCache<T> : SimpleCacheBase<T, ArrayContainer<Record<T>>>
        where T : IHasKey<int>
    {
        private readonly object _lock = new object();

        /// <summary>
        /// Constructor
        /// </summary>
        public SimpleArrayCache() : base(true)
        {
        }

        ///<inheritdoc/>
        protected override void StartLock()
        {
            Monitor.Enter(_lock);
        }

        ///<inheritdoc/>
        protected override void EndLock()
        {
            Monitor.Exit(_lock);
        }
    }
}
