using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using static NetGameShared.Util.Physical.Collision;
using Comps = NetGameShared.Ecs.Components;

namespace NetGameShared.Ecs.Systems.Collision
{
    public static class SolidToSolid
    {
        // Make this function inline so we don't have the overhead of passing in
        // a ton of parameters
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleCollision(
            Ecs.Registry registry,
            Queue<Entity> toRemove,
            Entity entity1,
            Comps.Solid entity1Solid,
            Comps.Position entity1Pos,
            Comps.Shapes.Rectangle entity1Rect,
            Entity entity2,
            Comps.Solid entity2Solid,
            Comps.Position entity2Pos,
            Comps.Shapes.Rectangle entity2Rect
        ) {
            if (entity1Solid.ephemeral) {
                toRemove.Enqueue(entity1);
                return;
            }

            if (entity2Solid.ephemeral) {
                toRemove.Enqueue(entity2);
                return;
            }

            Vector2 correction = GetMinCorrection(
                entity1Rect.data, entity1Pos.data,
                entity2Rect.data, entity2Pos.data
            );

            // Ignore the case where both are unmovable
            if (entity1Solid.unmovable) {
                entity2Pos.data -= correction;
            } else if (entity2Solid.unmovable) {
                entity1Pos.data += correction;
            } else {
                entity1Pos.data += correction / 2f;
                entity2Pos.data -= correction / 2f;
            }
        }

        private static void ProcessRemoves(
            Queue<Entity> toRemove,
            Ecs.Registry registry
        ) {
            while (toRemove.Count > 0) {
                Entity entity = toRemove.Dequeue();
                registry.Remove(entity);
            }
        }

        public static void Run(Ecs.Registry registry)
        {
            // To avoid performance costs from doing list intersections, only
            // query for entities with `Solid` components.
            //
            // However, we can assume that all entities with a `Solid` component
            // also have these components:
            // - `Position`
            // - `Shape.Rectangle`
            List<Entity> entities1 = registry.GetEntitiesList(typeof(Comps.Solid));

            // Use a `HashSet` to make removing entities O(1)
            var entities2 = new HashSet<Entity>(entities1);

            // Queue entities to remove so we aren't removing entities while
            // iterating over them
            Queue<Entity> toRemove = new Queue<Entity>();

            foreach (Ecs.Entity entity1 in entities1)
            {
                var entity1Pos = registry.GetComponentUnsafe<Comps.Position>(entity1);

                foreach (Ecs.Entity entity2 in entities2)
                {
                    var entity2Pos = registry.GetComponentUnsafe<Comps.Position>(entity2);

                    if (!IsCollisionPossible(entity1Pos.data, entity2Pos.data) ||
                        entity1.Equals(entity2)
                    ) {
                        continue;
                    }

                    var entity1Rect = registry.GetComponentUnsafe<Comps.Shapes.Rectangle>(entity1);
                    var entity2Rect = registry.GetComponentUnsafe<Comps.Shapes.Rectangle>(entity2);

                    if (!entity1.Equals(entity2) &&
                        CheckCollision(
                            entity1Rect.data, entity1Pos.data,
                            entity2Rect.data, entity2Pos.data
                        )
                    ) {
                        var entity1Solid = registry.GetComponentUnsafe<Comps.Solid>(entity1);
                        var entity2Solid = registry.GetComponentUnsafe<Comps.Solid>(entity2);
                        HandleCollision(
                            registry, toRemove,
                            entity1, entity1Solid, entity1Pos, entity1Rect,
                            entity2, entity2Solid, entity2Pos, entity2Rect
                        );
                    }
                }

                entities2.Remove(entity1);
            }

            ProcessRemoves(toRemove, registry);
        }
    }
}
