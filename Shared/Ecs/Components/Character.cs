using LiteNetLib.Utils;

using Tick = System.UInt32;

namespace NetGameShared.Ecs.Components
{
    public class Character : INetComponent<Character>
    {
        public enum Kind
        {
            Link,
            BlueLink,
            PurpleLink,
            RedLink,
            OrangeLink,
            WhiteLink,
            BlackLink,
            YellowLink,
            TealLink,
            PinkLink,
            Aquamentus,
            Zol,
            Goriya,
            Dodongos,
            Stalfos,
            Rope,
            Keese,
            Wallmaster
        }

        // Define sub-ranges of `Kind`
        public static class KindRanges
        {
            // `start` and `end` are inclusive

            // TODO: Add more player characters (e.g. colored Link variants)
            public static (Kind start, Kind end) players =
                (Kind.Link, Kind.PinkLink);

            public static (Kind start, Kind end) enemies =
                (Kind.Aquamentus, Kind.Wallmaster);
        }


        public Kind kind;

        public bool attacking = false;
        public Tick attackStartTick;

        public override string ToString()
        {
            return base.ToString() +
                ": { kind: " + kind +
                ", attacking: " + attacking +
                ", attackStartTick: " + attackStartTick + " }";
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put((int)kind);
            writer.Put(attacking);
            writer.Put(attackStartTick);
        }

        public void Deserialize(NetDataReader reader)
        {
            kind = (Kind)reader.GetInt();
            attacking = reader.GetBool();
            attackStartTick = reader.GetUInt();
        }

        public override int GetHashCode() {
            return (kind, attacking, attackStartTick).GetHashCode();
        }

        public override bool Equals(object obj) {
            return Equals(obj as Character);
        }

        public bool Equals(Character character) {
            return
                character != null &&
                character.kind == this.kind &&
                character.attacking == this.attacking &&
                character.attackStartTick == this.attackStartTick;
        }

        public object Clone()
        {
            return new Character {
                kind = this.kind,
                attacking = this.attacking,
                attackStartTick = this.attackStartTick
            };
        }
    }
}
