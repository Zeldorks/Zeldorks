using System;
using System.Collections.Generic;
using System.Linq;
using LiteNetLib.Utils;
using static NetGameShared.Net.Protocol.Serialization.Collections;

namespace NetGameShared.Net.Protocol
{
    public class EcDictDelta<Comp> : INetSerializable
        where Comp : Ecs.INetComponent<Comp>, new()
    {
        public Dictionary<Ecs.Entity, Comp> toAdd;
        public Dictionary<Ecs.Entity, Comp> toUpdate;
        public HashSet<Ecs.Entity> toRemove;

        public EcDictDelta()
        {
            toAdd = new Dictionary<Ecs.Entity, Comp>();
            toUpdate = new Dictionary<Ecs.Entity, Comp>();
            toRemove = new HashSet<Ecs.Entity>();
        }

        public EcDictDelta(
            EcDict<Comp> currentEcDict,
            EcDict<Comp> baseEcDict
        ) : this() {
            Dictionary<Ecs.Entity, Comp> currentData = currentEcDict.data;
            Dictionary<Ecs.Entity, Comp> baseData = baseEcDict.data;

            var entitiesToAdd = currentData.Keys.Except(baseData.Keys);
            foreach (Ecs.Entity entity in entitiesToAdd) {
                toAdd.Add(entity, currentData[entity]);
            }

            var entitiesToRemove = baseData.Keys.Except(currentData.Keys);
            foreach (Ecs.Entity entity in entitiesToRemove) {
                toRemove.Add(entity);
            }

            var entitiesToUpdate = currentData.Keys.Intersect(baseData.Keys);
            foreach (Ecs.Entity entity in entitiesToUpdate) {
                Comp currentComp = currentData[entity];
                Comp baseComp = baseData[entity];
                if (!currentComp.Equals(baseComp)) {
                    toUpdate.Add(entity, currentComp);
                }
            }
        }

        public void Serialize(NetDataWriter writer)
        {
            SerializeDictionary(writer, toAdd);
            SerializeDictionary(writer, toUpdate);
            SerializeHashSet(writer, toRemove);
        }

        public void Deserialize(NetDataReader reader)
        {
            toAdd = DeserializeDictionary<Ecs.Entity, Comp>(reader);
            toUpdate = DeserializeDictionary<Ecs.Entity, Comp>(reader);
            toRemove = DeserializeHashSet<Ecs.Entity>(reader);
        }
    }
}
