using System;
using LiteNetLib;
using Optional;
using static NetGameShared.Constants.Server.Timing;
using Tick = System.UInt32;
using ClientId = System.Int32;

namespace NetGameServer.Net
{
    public class Client
    {
        public NetPeer netPeer;

        // Tick of last game snapshot acked by client
        public Option<Tick> ackedTickOpt;

        public enum SnapRateState
        {
            // Occurs before the client acks the first game snapshot. Game
            // snapshots are sent at a slower rate because delta compression
            // cannot be used.
            Initial,

            // When the client is acking snapshots at a reasonable rate, delta
            // compression can be used, so we send game snapshots as quickly as
            // possible.
            Full,

            // Occurs when the client does not ack a game snapshot for a long
            // time and the last snapshot acked by the client was discarded from
            // the snapshot cache. During this state, game snapshots are sent at
            // a slower rate.
            Recovery
        }
        public SnapRateState snapRateState;

        public void SetSnapRateState(SnapRateState snapRateState) {
            this.snapRateState = snapRateState;
        }

        public Client(NetPeer netPeer)
        {
            this.netPeer = netPeer;
            snapRateState = SnapRateState.Initial;
            ackedTickOpt = Option.None<Tick>();
        }

        public ClientId Id
        {
            get { return netPeer.Id; }
        }

        // Snap rates: Number of ticks between sending game snapshots
        private Tick SnapshotSendPeriod
        {
            get {
                switch (snapRateState) {
                    case SnapRateState.Initial:
                        return ToTicks(TimeSpan.FromMilliseconds(200));
                    case SnapRateState.Recovery:
                        return ToTicks(TimeSpan.FromSeconds(1));
                    default:
                        // Send as many as possible
                        return 1;
                }
            }
        }

        public bool ShouldSendSnapshot
        {
            get { return Global.tick % SnapshotSendPeriod == 0; }
        }
    }
}
