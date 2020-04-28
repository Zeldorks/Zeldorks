using System;
using LiteNetLib.Utils;

namespace NetGameShared.Ecs.Components
{
    // TODO: Send this in game snapshot
    public class Solid : INetComponent<Solid>
    {
        // TODO: Add `mass` field or something similar

        // Delete the project on collision
        public bool ephemeral = false;
        public bool unmovable = false;

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(ephemeral);
            writer.Put(unmovable);
        }

        public void Deserialize(NetDataReader reader)
        {
            ephemeral = reader.GetBool();
            unmovable = reader.GetBool();
        }

        public override int GetHashCode() {
            return (ephemeral, unmovable).GetHashCode();
        }

        public override bool Equals(object obj) {
            return Equals(obj as Solid);
        }

        public bool Equals(Solid solid) {
            return
                solid != null &&
                solid.ephemeral == this.ephemeral &&
                solid.unmovable == this.unmovable;
        }

        public object Clone()
        {
            return new Solid {
                ephemeral = this.ephemeral,
                unmovable = this.unmovable
            };
        }
    }
}
