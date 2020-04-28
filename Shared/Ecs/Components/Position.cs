using System;
using LiteNetLib.Utils;
using PhysicalVector2 = Microsoft.Xna.Framework.Vector2;

namespace NetGameShared.Ecs.Components
{
    public class Position : INetComponent<Position>
    {
        // The center position of an entity
        public PhysicalVector2 data;

        public override string ToString()
        {
            return base.ToString() +
                ": { X: " + data.X + ", Y: " + data.Y + " }";
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(data.X);
            writer.Put(data.Y);
        }

        public void Deserialize(NetDataReader reader)
        {
            data.X = reader.GetFloat();
            data.Y = reader.GetFloat();
        }

        public override int GetHashCode() {
            return data.GetHashCode();
        }

        public override bool Equals(object obj) {
            return Equals(obj as Position);
        }

        public bool Equals(Position position) {
            return position != null && position.data == this.data;
        }

        public Object Clone()
        {
            return new Position {
                data = this.data
            };
        }
    }
}
