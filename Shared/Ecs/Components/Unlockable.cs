using LiteNetLib.Utils;

namespace NetGameShared.Ecs.Components
{
    public class Unlockable : INetComponent<Unlockable>
    {

        
        public enum Kind
        {      
                BlueDoor,
                RedDoor,
                GreenDoor,
                PurpleDoor,
                YellowDoor
        }
        public int number;
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
            return Equals(obj as Unlockable);
        }

        public bool Equals(Unlockable unlockable)
        {
            return unlockable != null && unlockable.kind == this.kind;
        }

        public object Clone()
        {
            return new Unlockable
            {
                kind = this.kind
            };
        }
    }
}
