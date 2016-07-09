using System;
using System.Collections.Generic;
using System.Linq;

namespace SomeKit.Cache.Container
{
    /// <summary>
    /// Implements <see cref="IContainer{T}"/> via <see cref="ContainerBase{T}"/> utilizing dictionaries
    /// </summary>
    /// <typeparam name="T">The type of elements in the container</typeparam>
    /// <typeparam name="TDictionary">The cictionary type representing inner storage</typeparam>
    public class DictionaryContainerBase<T, TDictionary> :
        ContainerBase<T>
        where TDictionary : IDictionary<int, T>, new()
    {
        private TDictionary _data;
        ///<inheritdoc/>
        public override int Size => _data.Keys.Max();
        ///<inheritdoc/>
        public override bool TryGet(int position, out T value)
        {
            if (position < 0)
                throw new IndexOutOfRangeException();
            value = default(T);
            if (_data == null || !_data.ContainsKey(position))
                return false;
            value = _data[position];
            return true;
        }
        ///<inheritdoc/>
        public override void Set(int position, T value)
        {
            if (position < 0)
                throw new IndexOutOfRangeException();
            if (_data == null)
                _data = new TDictionary();
            _data[position] = value;
        }
        ///<inheritdoc/>
        public override bool TryRemove(int position)
        {
            if (position < 0)
                throw new IndexOutOfRangeException();
            if (_data == null || !_data.ContainsKey(position) || _data[position].Equals(default(T)))
                return false;
            _data[position] = default(T);
            return true;
        }
        ///<inheritdoc/>
        public override void Clear()
        {
            _data.Clear();
        }
    }

    /// <summary>
    /// Implements <see cref="DictionaryContainerBase{T,TDictionary}"/> utilizing a basic unlocked <see cref="DictionaryContainer{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class DictionaryContainer<T> :
        DictionaryContainerBase<T, Dictionary<int, T>>
    {
    }
    //public sealed class ConcurrentDictionaryContainer<T> :
    //    DictionaryContainerBase<T, ConcurrentDictionary<int, T>>
    //{
    //}
}
