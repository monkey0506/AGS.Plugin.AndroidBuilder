using System;
using System.Collections;
using System.Collections.Generic;

namespace AGS.Plugin.AndroidBuilder
{
    /// <summary>
    /// Shallow wrapper around a dictionary, to prevent modifications to the dictionary.
    /// From .NET 4.5, this can be replaced with System.Collections.ObjectModel.ReadOnlyDictionary&lt;TKey, TValue&gt;.
    /// </summary>
    public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        /// <summary>
        /// The exception thrown if an attempt is made to modify a ReadOnlyDictionary.
        /// </summary>
        public class ReadOnlyException : NotSupportedException
        {
            public ReadOnlyException() : base("Collection is read-only.")
            {
            }
        }

        private IDictionary<TKey, TValue> _dict;

        /// <summary>
        /// Represents an empty ReadOnlyDictionary.
        /// </summary>
        public ReadOnlyDictionary()
        {
            _dict = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// Wraps the given dictionary.
        /// </summary>
        public ReadOnlyDictionary(IDictionary<TKey, TValue> dict)
        {
            _dict = dict;
        }

        public TValue this[TKey key]
        {
            get
            {
                return _dict[key];
            }
        }

        public int Count
        {
            get
            {
                return _dict.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                return _dict.Keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                return _dict.Values;
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dict.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return _dict.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _dict.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dict.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_dict).GetEnumerator();
        }

        TValue IDictionary<TKey, TValue>.this[TKey key]
        {
            get
            {
                return this[key];
            }

            set
            {
                throw new ReadOnlyException();
            }
        }

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            throw new ReadOnlyException();
        }

        bool IDictionary<TKey, TValue>.Remove(TKey key)
        {
            throw new ReadOnlyException();
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            throw new ReadOnlyException();
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            throw new ReadOnlyException();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new ReadOnlyException();
        }
    }
}
