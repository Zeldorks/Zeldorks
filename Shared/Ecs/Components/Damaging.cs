using LiteNetLib.Utils;

namespace NetGameShared.Ecs.Components
{
    // TODO: Send this in game snapshot
    public class Damaging : INetComponent<Damaging>
    {
        public int damage;
        public bool selfDamage = false;
        public uint attackerId;

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(damage);
            writer.Put(selfDamage);
            writer.Put(attackerId);
        }

        public void Deserialize(NetDataReader reader)
        {
            damage = reader.GetInt();
            selfDamage = reader.GetBool();
            attackerId = reader.GetUInt();
        }

        public override int GetHashCode() {
            return damage ^ selfDamage.GetHashCode() ^ (int)attackerId;
        }

        public override bool Equals(object obj) {
            return Equals(obj as Damaging);
        }

        public bool Equals(Damaging damaging) {
            return
                damaging != null &&
                damaging.damage == this.damage &&
                damaging.selfDamage == this.selfDamage &&
                damaging.attackerId == this.attackerId;
        }

        public object Clone()
        {
            return new Damaging
            {
                damage = damage,
                selfDamage = selfDamage,
                attackerId = attackerId
            };
        }
    }
}
