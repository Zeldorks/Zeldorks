using System;
using LiteNetLib.Utils;

namespace NetGameShared.Ecs
{
    public struct Entity : IEquatable<Entity>, INetSerializable
    {
        public uint id;

        public Entity(uint id)
        {
            this.id = id;
        }

        public override int GetHashCode() {
            return (int)id;
        }

        public override bool Equals(object obj)
        {
            if (obj is Entity) {
                return Equals((Entity)obj);
            } else {
                return false;
            }
        }

        public bool Equals(Entity entity) {
            return entity.id == this.id;
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(id);
        }

        public void Deserialize(NetDataReader reader)
        {
            id = reader.GetUInt();
        }
    }
}
