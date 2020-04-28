using System;
using NetGameShared.Util;
using static NetGameShared.Constants.Server.Timing;
using Packets = NetGameShared.Net.Protocol.Packets;
using Tick = System.UInt32;
using Optional;

namespace NetGameServer.GameNS
{
    public class GameSnapshotCache
    {
        // Cache for full game snapshots
        private Cache<Tick, Packets.GameSnapshot> data;

        // Dependencies
        private World world;

        public GameSnapshotCache(World world)
        {
            this.world = world;

            // Cache game snapshots for about 1 second. The actual time is
            // multipled by the `snapPeriod`.
            var maxSize = ToTicks(TimeSpan.FromSeconds(1));
            data = new Cache<Tick, Packets.GameSnapshot>(maxSize);
        }

        public bool ContainsGameSnapshot(Tick atTick)
        {
            return data.ContainsKey(atTick);
        }

        // Get a previous game snapshot stored in the cache
        public Option<Packets.GameSnapshot> GetCached(Tick atTick)
        {
            return data.Get(atTick);
        }

        // Get the current game snapshot. If it's not in the cache, create it
        // and then cache it.
        public Packets.GameSnapshot GetCurrent()
        {
            Option<Packets.GameSnapshot> cachedOpt = GetCached(Global.tick);
            return cachedOpt.Match(
                some: cached => { return cached; },
                none: () =>
                {
                    // Create new game snapshot
                    Packets.GameSnapshot gameSnapshot = world.ecsRegistry.Snap(Global.tick);
                    data.Add(Global.tick, gameSnapshot);
                    return gameSnapshot;
                }
            );
        }
    }
}
