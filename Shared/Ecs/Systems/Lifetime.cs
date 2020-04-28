using System.Collections.Generic;
using LifetimeComp = NetGameShared.Ecs.Components.Lifetime;

namespace NetGameShared.Ecs.Systems
{
    public static class Lifetime
    {
        public static void Run(Registry registry) {
            HashSet<Entity> entities = registry.GetEntities(
                typeof(LifetimeComp)
            );

            // Entities that have exceeded their lifetime and will be removed
            HashSet<Entity> toRemove = new HashSet<Entity>();

            foreach (Entity entity in entities) {
                LifetimeComp lifetimeComp = registry
                    .GetComponentUnsafe<LifetimeComp>(entity);

                if (lifetimeComp.ticks == 0) {
                    toRemove.Add(entity);
                } else {
                    lifetimeComp.ticks--;
                }
            }

            foreach (Entity entity in toRemove) {
                registry.Remove(entity);
            }
        }
    }
}
