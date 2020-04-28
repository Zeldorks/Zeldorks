using System;
using Tick = System.UInt32;

namespace NetGameShared.Constants.Server
{
    public static class Timing
    {
        public static readonly TimeSpan tickTimeSpan
            = TimeSpan.FromMilliseconds(20);

        public static Tick ToTicks(TimeSpan timeSpan)
        {
            return (uint)(timeSpan.Ticks / tickTimeSpan.Ticks);
        }
    }
}
