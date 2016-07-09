using System;

namespace SomeKit.Cache
{
    public class Record<T>
    {
        public Record(T item)
        {
            Created = Modified = DateTime.Now;
            _item = item;
        }
        public DateTime Created { get; }
        public DateTime Modified { get; private set; }
        public DateTime? LastRead { get; private set; }

        private T _item;
        public T Item
        {
            get
            {
                LastRead = DateTime.Now;
                return _item;
            }
            set
            {
                _item = value;
                Modified = DateTime.Now;
            }
        }
    }
}
