using System.Collections.Generic;
using Optional;

namespace NetGameShared.Util
{
    public class Cache<K, V>
    {
        public Dictionary<K, V> data;
        public Queue<K> insertOrder;
        public uint maxSize;

        public Cache(uint maxSize)
        {
            data = new Dictionary<K, V>((int)maxSize);
            insertOrder = new Queue<K>((int)maxSize);
            this.maxSize = maxSize;
        }

        public Option<V> Get(K key)
        {
            if (data.ContainsKey(key))
            {
                return data[key].Some();
            }
            else
            {
                return Option.None<V>();
            }
        }

        public void Add(K key, V value)
        {
            data.Add(key, value);
            insertOrder.Enqueue(key);
            while (data.Count > maxSize)
            {
                K toRemove = insertOrder.Dequeue();
                data.Remove(toRemove);
            }
        }

        public void Clear()
        {
            data.Clear();
            insertOrder.Clear();
        }

        public bool ContainsKey(K key)
        {
            return data.ContainsKey(key);
        }

        public int Count
        {
            get { return data.Count; }
        }
    }
}
