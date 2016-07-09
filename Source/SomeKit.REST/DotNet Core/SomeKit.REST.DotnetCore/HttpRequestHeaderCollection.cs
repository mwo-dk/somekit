using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SomeKit.REST
{
    ///<inheritdoc/>
    public sealed class HttpRequestHeaderCollection : IHttpRequestHeaderCollection
    {
        private readonly Collection<IHttpRequestHeader> _innerCollection; 
        internal HttpRequestHeaderCollection()
        {
            _innerCollection = new Collection<IHttpRequestHeader>();
        }

        ///<inheritdoc/>
        public int Count => _innerCollection.Count;

        ///<inheritdoc/>
        public bool IsReadOnly => false;

        ///<inheritdoc/>
        public void Add(IHttpRequestHeader item)
        {
            _innerCollection.Add(item);
        }
        ///<inheritdoc/>
        public void Clear()
        {
            _innerCollection.Clear();
        }
        ///<inheritdoc/>
        public bool Contains(IHttpRequestHeader item)
        {
            return _innerCollection.Contains(item);
        }
        ///<inheritdoc/>
        public void CopyTo(IHttpRequestHeader[] array, int arrayIndex)
        {
            _innerCollection.CopyTo(array, arrayIndex);
        }
        ///<inheritdoc/>
        public IEnumerator<IHttpRequestHeader> GetEnumerator()
        {
            return _innerCollection.GetEnumerator();
        }
        ///<inheritdoc/>
        public bool Remove(IHttpRequestHeader item)
        {
            return _innerCollection.Remove(item);
        }
        ///<inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _innerCollection.GetEnumerator();
        }
    }
}
