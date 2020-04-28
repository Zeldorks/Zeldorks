using LiteNetLib.Utils;

namespace NetGameShared.Ecs.Components
{
    public class Item : INetComponent<Item>
    {
        public enum Kind
        {
            Heart,
            Compass,
            Bow,
            Sword,
            Bomb,
            Boomerang,
            Clock,
            Rupee,
            TriforceShard,
            Candle,
            Map,
            RedKey,
            BlueKey,
            GreenKey,
            PurpleKey,
            YellowKey
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

        public override int GetHashCode() {
            return (int)kind;
        }

        public override bool Equals(object obj) {
            return Equals(obj as Item);
        }

        public bool Equals(Item item) {
            return item != null && item.kind == this.kind;
        }

        public object Clone()
        {
            return new Item {
                kind = this.kind
            };
        }
    }
}
