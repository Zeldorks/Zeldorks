using System;
using LiteNetLib.Utils;

using ClientId = System.Int32;

namespace NetGameShared.Ecs.Components
{
    public class Playable : INetComponent<Playable>
    {
        // The client ID that is in control
        public ClientId clientId;

        public override string ToString()
        {
            return base.ToString() +
                ": { clientId: " + clientId + " }";
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(clientId);
        }

        public void Deserialize(NetDataReader reader)
        {
            clientId = reader.GetInt();
        }

        public override int GetHashCode()
        {
            return clientId;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Playable);
        }

        public bool Equals(Playable playable)
        {
            return playable != null && playable.clientId == this.clientId;
        }

        public object Clone()
        {
            return new Playable
            {
                clientId = this.clientId
            };
        }
    }
}
