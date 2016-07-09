using System;
using System.Collections.Generic;
using System.Linq;

namespace SomeKit.Cache.Container
{
    public class DictionaryContainerBase<T, TDictionary> :
        ContainerBase<T>
        where TDictionary : IDictionary<int, T>, new()
    {
        private TDictionary _data;
        public override int Size => _data.Keys.Max();

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

        public override void Set(int position, T value)
        {
            if (position < 0)
                throw new IndexOutOfRangeException();
            if (_data == null)
                _data = new TDictionary();
            _data[position] = value;
        }

        public override bool TryRemove(int position)
        {
            if (position < 0)
                throw new IndexOutOfRangeException();
            if (_data == null || !_data.ContainsKey(position) || _data[position].Equals(default(T)))
                return false;
            _data[position] = default(T);
            return true;
        }

        public override void Clear()
        {
            _data.Clear();
        }
    }

    public sealed class DictionaryContainer<T> :
        DictionaryContainerBase<T, Dictionary<int, T>>
    {
    }
    //public sealed class ConcurrentDictionaryContainer<T> :
    //    DictionaryContainerBase<T, ConcurrentDictionary<int, T>>
    //{
    //}
}
