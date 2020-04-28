using System.Collections.Generic;
using LiteNetLib.Utils;
using static NetGameShared.Net.Protocol.Serialization.Collections;

namespace NetGameShared.Net.Protocol
{
    // Entity to component dictionary
    public class EcDict<Comp> : INetSerializable
        where Comp : Ecs.INetComponent<Comp>, new()
    {
        public Dictionary<Ecs.Entity, Comp> data;

        public EcDict()
        {
            data = new Dictionary<Ecs.Entity, Comp>();
        }

        public void Serialize(NetDataWriter writer)
        {
            SerializeDictionary(writer, data);
        }

        public void Deserialize(NetDataReader reader)
        {
            data = DeserializeDictionary<Ecs.Entity, Comp>(reader);
        }
    }
}
