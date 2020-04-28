using LiteNetLib.Utils;

using Physical = NetGameShared.Util.Physical;
using PhysicalVector2 = Microsoft.Xna.Framework.Vector2;

namespace NetGameShared.Ecs.Components.Orientations
{
    public class Cardinal : Orientation, INetComponent<Cardinal>
    {
        public Physical.Orientation.Cardinal data;

        public override string ToString()
        {
            return base.ToString() +
                ": { direction: " + data + " }";
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((int)data);
        }

        public void Deserialize(NetDataReader reader)
        {
            data = (Physical.Orientation.Cardinal)reader.GetInt();
        }

        public override int GetHashCode() {
            return (int)data;
        }

        public override bool Equals(object obj) {
            return Equals(obj as Cardinal);
        }

        public bool Equals(Cardinal cardinal) {
            return cardinal != null && cardinal.data == this.data;
        }

        public object Clone()
        {
            return new Cardinal {
                data = this.data
            };
        }
    }
}
