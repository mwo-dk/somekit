using System;

namespace SomeKit.Cache
{
    /// <summary>
    /// Represents a cache record of a given item of <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">The type of element in the record</typeparam>
    public class Record<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="item">The item to store in the record</param>
        public Record(T item)
        {
            Created = Modified = DateTime.Now;
            _item = item;
        }
        /// <summary>
        /// Creation time of the given record
        /// </summary>
        public DateTime Created { get; }
        /// <summary>
        /// Time of last modification of  the given record
        /// </summary>
        public DateTime Modified { get; private set; }
        /// <summary>
        /// Last reading time of a given record
        /// </summary>
        public DateTime? LastRead { get; private set; }

        private T _item;
        /// <summary>
        /// The item in the record
        /// </summary>
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
