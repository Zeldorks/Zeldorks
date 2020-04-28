using System.Collections.Generic;
using LiteNetLib.Utils;

namespace NetGameShared.Net.Protocol.Serialization
{
    public static class Collections
    {
        // TODO: Use `NetPacketProcessor.RegisterNestedType` on these?

        public static void SerializeDictionary<K, V>(
            NetDataWriter writer,
            Dictionary<K, V> dict
        )
            where K : INetSerializable
            where V : INetSerializable
        {
            int count = dict.Count;
            writer.Put(count);
            foreach (KeyValuePair<K, V> pair in dict)
            {
                pair.Key.Serialize(writer);
                pair.Value.Serialize(writer);
            }
        }

        public static Dictionary<K, V> DeserializeDictionary<K, V>(
            NetDataReader reader
        )
            where K : INetSerializable, new()
            where V : INetSerializable, new()
        {
            int count = reader.GetInt();
            var result = new Dictionary<K, V>(count);
            for (int i = 0; i < count; i++)
            {
                K key = new K();
                V value = new V();
                key.Deserialize(reader);
                value.Deserialize(reader);
                result.Add(key, value);
            }

            return result;
        }

        public static void SerializeHashSet<T>(
            NetDataWriter writer,
            HashSet<T> hashSet
        )
            where T : INetSerializable
        {
            int count = hashSet.Count;
            writer.Put(count);
            foreach (T t in hashSet)
            {
                t.Serialize(writer);
            }
        }

        public static HashSet<T> DeserializeHashSet<T>(
            NetDataReader reader
        )
            where T : INetSerializable, new()
        {
            int count = reader.GetInt();
            var result = new HashSet<T>();
            for (int i = 0; i < count; i++)
            {
                T t = new T();
                t.Deserialize(reader);
                result.Add(t);
            }

            return result;
        }
    }
}
