using System;
using System.Collections.Generic;
using static NetGameShared.Util.Physical.Collision;
using Comps = NetGameShared.Ecs.Components;

using PhysicalVector2 = Microsoft.Xna.Framework.Vector2;

namespace NetGameShared.Ecs.Systems.Collision
{
    public static class Items
    {
        private static void ProcessRemoves(
            Queue<Entity> toRemove,
            Ecs.Registry registry
        ) {
            while (toRemove.Count > 0) {
                Entity entity = toRemove.Dequeue();
                registry.Remove(entity);
            }
        }

        public static void Run(Registry registry)
        {
            // To avoid performance costs from doing list intersections, only
            // query for entities with `Item` and `Inventory` components.
            //
            // However, we can assume that these entities also have these
            // components:
            // - `Position`
            // - `Shape.Rectangle`
            // - `Orientation.Cardinal`
            List<Entity> itemEntities = registry.GetEntitiesList(typeof(Comps.Item));
            List<Entity> entities = registry.GetEntitiesList(typeof(Comps.Inventory));

            // Queue entities to remove so we aren't removing entities while
            // iterating over them
            Queue<Entity> toRemove = new Queue<Entity>();

            foreach (Entity itemEntity in itemEntities) {
                var itemPositionComp = registry.GetComponentUnsafe<Comps.Position>(itemEntity);

                foreach (Entity entity in entities) {
                    var positionComp = registry.GetComponentUnsafe<Comps.Position>(entity);

                    if (!IsCollisionPossible(itemPositionComp.data, positionComp.data)) {
                        continue;
                    }

                    var itemRectangleComp = registry.GetComponentUnsafe<Comps.Shapes.Rectangle>(itemEntity);
                    var rectangleComp = registry.GetComponentUnsafe<Comps.Shapes.Rectangle>(entity);
                    var orientationComp = registry.GetComponentUnsafe<Comps.Orientations.Cardinal>(entity);

                    bool hasCollision = CheckCollision(
                        itemRectangleComp.data, itemPositionComp.data, Util.Physical.Orientation.Cardinal.Right,
                        rectangleComp.data, positionComp.data, orientationComp.data
                    );

                    if (hasCollision) {
                        var itemComp = registry.GetComponentUnsafe<Comps.Item>(itemEntity);
                        var inventoryComp = registry.GetComponentUnsafe<Comps.Inventory>(entity);

                        if (inventoryComp.TryTakeItem(itemComp.kind)) {
                            switch (itemComp.kind) {
                                case Comps.Item.Kind.Heart:
                                    Console.WriteLine(
                                        "Entity {0} got health item, now has {1} HP",
                                        entity.id,
                                        inventoryComp.data[Comps.Item.Kind.Heart].Count
                                    );
                                    break;
                            }

                            toRemove.Enqueue(itemEntity);
                        }
                    }
                }
            }

            ProcessRemoves(toRemove, registry);
        }
    }
}
