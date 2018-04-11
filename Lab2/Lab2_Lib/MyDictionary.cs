using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lab2_Lib
{
    public class MyDictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>
    {
        class DefaultComparer : IEqualityComparer<TKey>
        {
            public bool Equals(TKey x, TKey y)
            {
                return x.Equals(y);
            }

            public int GetHashCode(TKey obj)
            {
                return obj.GetHashCode();
            }
        }

        private const float MULTIPLIER = 2;
        private const int DEFAULT_INITIAL_SIZE = 20;

        private KeyValuePair<TKey, TValue>[] _collection = new KeyValuePair<TKey, TValue>[DEFAULT_INITIAL_SIZE];
        private int _count = 0;
        private IEqualityComparer<TKey> _comparer = new DefaultComparer();

        private int indexOf(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key is null");
            for (int i = 0; i < _count; i++) {
                if (_comparer.Equals(_collection[i].Key, key))
                    return i;
            }
            return -1;
        }

        #region events
        public event EventHandler<MyDictionaryEventArgs<TKey,TValue>> OnAdd;
        public event EventHandler<MyDictionaryEventArgs<TKey, TValue>> OnRemove;
        public event EventHandler<MyDictionaryEventArgs<TKey, TValue>> OnClear;
        #endregion

        #region constructors

        public MyDictionary() { }

        public MyDictionary(IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary.Count > DEFAULT_INITIAL_SIZE)
                _collection = new KeyValuePair<TKey, TValue>[dictionary.Count];
            dictionary.CopyTo(_collection, 0);
        }

        public MyDictionary(int capacity)
        {
            _collection = new KeyValuePair<TKey, TValue>[capacity];
        }

        public MyDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            _collection = collection.ToArray();
        }

        public MyDictionary(IEqualityComparer<TKey> comparer)
        {
            Comparer = comparer;
        }

        public MyDictionary(int capacity, IEqualityComparer<TKey> comparer) : this(capacity)
        {
            Comparer = comparer;
        }

        public MyDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : this(dictionary)
        {
            Comparer = comparer;
        }

        public MyDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer) : this(collection)
        {
            Comparer = comparer;
        }

        #endregion

        #region interface implementation

        public TValue this[TKey key]
        {
            get {
                TValue value;
                if (!TryGetValue(key, out value))
                    throw new KeyNotFoundException();
                return value;
            }
            set {
                var index = indexOf(key);
                if (index == -1)
                    throw new KeyNotFoundException();
                _collection[index] = new KeyValuePair<TKey, TValue>(key, value);
            }
        }

        public IEqualityComparer<TKey> Comparer
        {
            get {
                return _comparer;
            }
            set {
                if (value == null)
                    _comparer = new DefaultComparer();
                else _comparer = value;
            }
        }

        public int Count => _count;

        public bool IsReadOnly => false;

        public ICollection<TKey> Keys => _collection.Take(_count).Select(x => x.Key).ToArray();

        public ICollection<TValue> Values => _collection.Take(_count).Select(x => x.Value).ToArray();

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            if (indexOf(item.Key) != -1)
                throw new ArgumentException("An element with the same key already exists in the");
            if (_count == _collection.Length) {
                var temp = _collection;
                _collection = new KeyValuePair<TKey, TValue>[(int)(_collection.Length * MULTIPLIER)];
                temp.CopyTo(_collection, 0);
            }
            _collection[_count++] = item;

            OnAdd?.Invoke(this, new MyDictionaryEventArgs<TKey, TValue>(item));
        }

        public void Add(TKey key, TValue value)
        {
            Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        public void Clear()
        {
            _count = 0;
            _collection = new KeyValuePair<TKey, TValue>[DEFAULT_INITIAL_SIZE];
            OnClear?.Invoke(this, null);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return TryGetValue(item.Key, out TValue value) &&
                (value == null && item.Value == null || value.Equals(item.Value));
        }

        public bool ContainsKey(TKey key)
        {
            return indexOf(key) != -1;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("arrayIndex can't be less than 0");
            if (array == null)
                throw new ArgumentNullException("Array can't be null");
            if (array.Length < _count + arrayIndex)
                throw new ArgumentException("Destination array was not long enough");
            for (int i = 0; i < _count; i++) {
                array[i + arrayIndex] = _collection[i];
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _collection.ToList().Take(_count).GetEnumerator();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            int index = indexOf(item.Key);
            if (index != -1 && _collection[index].Value.Equals(item.Value)) {
                _count--;
                OnRemove?.Invoke(this, new MyDictionaryEventArgs<TKey, TValue>(_collection[index]));
                _collection[index] = _collection[_count];
                return true;
            }
            return false;
        }

        public bool Remove(TKey key)
        {
            int index = indexOf(key);
            if (index != -1) {
                _count--;
                OnRemove?.Invoke(this, new MyDictionaryEventArgs<TKey, TValue>(_collection[index]));
                if (_count == 0) {
                    _collection[index] = default(KeyValuePair<TKey, TValue>);
                }
                _collection[index] = _collection[_count];
                return true;
            }
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            int index = indexOf(key);
            if (index == -1) {
                value = default(TValue);
                return false;
            }
            value = _collection[index].Value;
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _collection.Take(_count).GetEnumerator();
        }

        #endregion

        public bool ContainsValue(TValue value)
        {
            for (int i = 0; i < _count; i++)
                if (_collection[i].Value.Equals(value))
                    return true;
            return false;
        }

        public bool TryAdd(TKey key, TValue value)
        {
            if (indexOf(key) != -1)
                return false;
            Add(key, value);
            return true;
        }

        public bool TryAdd(KeyValuePair<TKey, TValue> item)
        {
            return TryAdd(item.Key, item.Value);
        }
    }
}
