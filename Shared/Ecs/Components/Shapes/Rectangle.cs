using LiteNetLib.Utils;
using Physical = NetGameShared.Util.Physical;

namespace NetGameShared.Ecs.Components.Shapes
{
    public class Rectangle : Shape, INetComponent<Rectangle>
    {
        public Physical.Rectangle data;

        public override string ToString()
        {
            return base.ToString() +
                ": { Width: " + data.Width +
                ", Height: " + data.Height + " }";
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(data.Width);
            writer.Put(data.Height);
        }

        public void Deserialize(NetDataReader reader)
        {
            data.Width = reader.GetFloat();
            data.Height = reader.GetFloat();
        }

        public override int GetHashCode() {
            return data.GetHashCode();
        }

        public override bool Equals(object obj) {
            return Equals(obj as Rectangle);
        }

        public bool Equals(Rectangle rectangle) {
            return rectangle != null && rectangle.data == this.data;
        }

        public object Clone()
        {
            return new Rectangle {
                data = this.data
            };
        }
    }
}
