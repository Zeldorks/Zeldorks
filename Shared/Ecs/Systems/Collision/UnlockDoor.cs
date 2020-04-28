using System;
using System.Collections.Generic;
using static NetGameShared.Util.Physical.Collision;
using Comps = NetGameShared.Ecs.Components;

namespace NetGameShared.Ecs.Systems.Collision
{
    public static class UnlockDoor
    {
        private static bool CanUnlock(
            Comps.Projectile.Kind projectileKind,
            Comps.Door.Kind doorKind
        ) {
            switch (doorKind) {
                case Comps.Door.Kind.Red:
                    return projectileKind == Comps.Projectile.Kind.RedKey;
                case Comps.Door.Kind.Green:
                    return projectileKind == Comps.Projectile.Kind.GreenKey;
                case Comps.Door.Kind.Blue:
                    return projectileKind == Comps.Projectile.Kind.BlueKey;
                case Comps.Door.Kind.Purple:
                    return projectileKind == Comps.Projectile.Kind.PurpleKey;
                case Comps.Door.Kind.Yellow:
                    return projectileKind == Comps.Projectile.Kind.YellowKey;
                default:
                    throw new ArgumentException("Invalid door kind");
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
            // query for entities with `Projectile` and `Door` components.
            //
            // However, we can assume that these entities also have these
            // components:
            // - `Position`
            // - `Shape.Rectangle`
            // - `Orientation.Cardinal`
            List<Entity> projectileEntities = registry.GetEntitiesList(typeof(Comps.Projectile));
            List<Entity> doorEntities = registry.GetEntitiesList(typeof(Comps.Door));

            // Queue entities to remove so we aren't removing entities while
            // iterating over them
            Queue<Entity> toRemove = new Queue<Entity>();

            foreach (Ecs.Entity projectileEntity in projectileEntities) {
                var projectilePosition = registry.GetComponentUnsafe<Comps.Position>(projectileEntity);

                foreach (Ecs.Entity doorEntity in doorEntities) {
                    var doorPosition = registry.GetComponentUnsafe<Comps.Position>(doorEntity);

                    if (!IsCollisionPossible(projectilePosition.data, doorPosition.data) ||
                        projectileEntity.Equals(doorEntity)
                    ) {
                        continue;
                    }

                    var projectileRectangle = registry.GetComponentUnsafe<Comps.Shapes.Rectangle>(projectileEntity);
                    var projectileOrientation = registry.GetComponentUnsafe<Comps.Orientations.Cardinal>(projectileEntity);
                    var projectile = registry.GetComponentUnsafe<Comps.Projectile>(projectileEntity);

                    var doorRectangle = registry.GetComponentUnsafe<Comps.Shapes.Rectangle>(doorEntity);
                    var doorOrientation = registry.GetComponentUnsafe<Comps.Orientations.Cardinal>(doorEntity);
                    var door = registry.GetComponentUnsafe<Comps.Door>(doorEntity);

                    if (CanUnlock(projectile.kind, door.kind) &&
                        CheckCollision(
                            doorRectangle.data, doorPosition.data, doorOrientation.data,
                            projectileRectangle.data, projectilePosition.data, projectileOrientation.data
                        )
                    ) {
                        toRemove.Enqueue(doorEntity);
                        Console.WriteLine(
                            "The {0} door has been opened",
                            door.kind
                        );
                    }
                }
            }

            ProcessRemoves(toRemove, registry);
        }
    }
}
