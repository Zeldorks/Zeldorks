using Tick = System.UInt32;

namespace NetGameShared.Net.Protocol.Packets
{
    public class GameSnapshotAck
    {
        public Tick AckedTick { get; set; }

        public GameSnapshotAck Clone()
        {
            return new GameSnapshotAck {
                AckedTick = this.AckedTick
            };
        }
    }
}
