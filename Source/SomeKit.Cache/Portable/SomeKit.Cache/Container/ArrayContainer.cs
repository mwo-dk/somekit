using System;

namespace SomeKit.Cache.Container
{
    /// <summary>
    /// Implements <see cref="IContainer{T}"/> via <see cref="ContainerBase{T}"/> utilizing basic arrays
    /// </summary>
    /// <typeparam name="T">The type of elements in the container</typeparam>
    public sealed class ArrayContainer<T> :
        ContainerBase<T>
    {
        private T[] _data;
        ///<inheritdoc/>
        public override int Size => _data?.Length ?? 0;
        ///<inheritdoc/>
        public override bool TryGet(int position, out T value)
        {
            if (position < 0)
                throw new IndexOutOfRangeException();
            value = default(T);
            if (_data == null || _data.Length <= position)
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
                _data = new T[GetNewSize(position)];
            if (_data.Length <= position)
                Array.Resize(ref _data, GetNewSize(position));
            _data[position] = value;
        }
        ///<inheritdoc/>
        public override bool TryRemove(int position)
        {
            if (position < 0)
                throw new IndexOutOfRangeException();
            if (_data == null || _data.Length <= position || _data[position].Equals(default(T)))
                return false;
            _data[position] = default(T);
            return true;
        }
        ///<inheritdoc/>
        public override void Clear()
        {
            _data = null;
        }

        private int GetNewSize(int n)
        {
            var result = 16;
            while (result <= n)
                result *= 2;
            return result;
        }
    }
}
