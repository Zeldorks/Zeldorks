using LiteNetLib.Utils;

namespace NetGameShared.Ecs.Components
{
    public class Projectile : INetComponent<Projectile>
    {
        public enum Kind
        {
            Arrow,
            SilverArrow,
            Bomb,
            Boomerang,
            MagicalBoomerang,
            SwordSlash,
            Sword,
            WhiteSword,
            RedKey,
            BlueKey,
            GreenKey,
            PurpleKey,
            YellowKey,
            Fireball
        }

        public Kind kind;

        // Owner refers to the one who fired the projectile
        public uint ownerId;

        public override string ToString()
        {
            return base.ToString() +
                ": { kind: " + kind +
                ", ownerClientId: " + ownerId + " }";
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((int)kind);
            writer.Put(ownerId);
        }

        public void Deserialize(NetDataReader reader)
        {
            kind = (Kind)reader.GetInt();
            ownerId = reader.GetUInt();
        }

        public override int GetHashCode() {
            return (int)kind ^ (int)ownerId;
        }

        public override bool Equals(object obj) {
            return Equals(obj as Projectile);
        }

        public bool Equals(Projectile projectile) {
            return
                projectile != null &&
                projectile.kind == this.kind &&
                projectile.ownerId == this.ownerId;
        }

        public object Clone()
        {
            return new Projectile {
                kind = this.kind,
                ownerId = this.ownerId
            };
        }
    }
}
