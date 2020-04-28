using System;
using LiteNetLib.Utils;
using PhyisicalVector2 = Microsoft.Xna.Framework.Vector2;

namespace NetGameShared.Ecs.Components
{
    // TODO: Send this in game snapshot
    public class ReturnVelocity : INetComponent<Velocity>
    {
        public PhyisicalVector2 data;

        // TODO: Maybe put this in a different component? I'm not a familiar
        // with physics so I'm not sure what the proper name for this field
        // should be.
        //
        // Simulate friction by decaying velocity.
        public float decay = 0f;

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

        public override int GetHashCode()
        {
            return data.GetHashCode() ^ decay.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Velocity);
        }

        public bool Equals(Velocity velocity)
        {
            return
                velocity != null &&
                velocity.data == this.data &&
                velocity.decay == this.decay;
        }

        public Object Clone()
        {
            return new Velocity
            {
                data = this.data,
                decay = this.decay
            };
        }
    }
}
