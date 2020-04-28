using Tick = System.UInt32;

namespace NetGameShared.Ecs.Components
{
    public class Lifetime : IComponent
    {
        // Lifetime in ticks
        public Tick ticks;
    }
}
