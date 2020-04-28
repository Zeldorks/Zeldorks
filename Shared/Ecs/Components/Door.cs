using LiteNetLib.Utils;

namespace NetGameShared.Ecs.Components
{
    public class Door : INetComponent<Door>
    {
        public enum Kind
        {
            Blue,
            Red,
            Green,
            Purple,
            Yellow
        }

        public Kind kind;

        public override string ToString()
        {
            return base.ToString() +
                ": { kind: " + kind + " }";
        }
        public void Serialize(NetDataWriter writer)
        {
            writer.Put((int)kind);
        }

        public void Deserialize(NetDataReader reader)
        {
            kind = (Kind)reader.GetInt();
        }

        public override int GetHashCode()
        {
            return (int)kind;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Door);
        }

        public bool Equals(Door door)
        {
            return door != null && door.kind == this.kind;
        }
        public object Clone()
        {
            return new Door
            {
                kind = this.kind
            };
        }
    }
}
