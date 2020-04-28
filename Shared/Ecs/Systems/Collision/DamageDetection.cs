using System;
using System.Collections.Generic;
using static NetGameShared.Util.Physical.Collision;
using Comps = NetGameShared.Ecs.Components;

namespace NetGameShared.Ecs.Systems.Collision
{
    public static class DamageDetection
    {
        public static void Run(Ecs.Registry registry)
        {
            // To avoid performance costs from doing list intersections, only
            // query for entities with `Damaging`, `Projectile`, and `Inventory` components.
            //
            // However, we can assume that these entities also have these
            // components:
            // - `Position`
            // - `Shape.Rectangle`
            // - `Orientation.Cardinal`
            HashSet<Entity> hitEntities = registry.GetEntities(
                typeof(Comps.Damaging),
                typeof(Comps.Projectile)
            );
            List<Entity> hurtEntities = registry.GetEntitiesList(typeof(Comps.Inventory));

            foreach (Ecs.Entity hitEntity in hitEntities) {
                var hitPosition = registry.GetComponentUnsafe<Comps.Position>(hitEntity);

                foreach (Ecs.Entity hurtEntity in hurtEntities) {
                    var hurtPosition = registry.GetComponentUnsafe<Comps.Position>(hurtEntity);

                    if (!IsCollisionPossible(hitPosition.data, hurtPosition.data) ||
                        hitEntity.Equals(hurtEntity)
                    ) {
                        continue;
                    }

                    var hitRectangle = registry.GetComponentUnsafe<Comps.Shapes.Rectangle>(hitEntity);
                    var hitOrientation = registry.GetComponentUnsafe<Comps.Orientations.Cardinal>(hitEntity);
                    var hitDamage = registry.GetComponentUnsafe<Comps.Damaging>(hitEntity);

                    var hurtRectangle = registry.GetComponentUnsafe<Comps.Shapes.Rectangle>(hurtEntity);
                    var hurtOrientation = registry.GetComponentUnsafe<Comps.Orientations.Cardinal>(hurtEntity);

                    // Only check for collision if the entity isn't being hit by
                    // its own attack (unless it has self damage on).
                    bool canDamage = (hurtEntity.id != hitDamage.attackerId || hitDamage.selfDamage);
                    if (canDamage &&
                        CheckCollision(
                            hurtRectangle.data, hurtPosition.data, hurtOrientation.data,
                            hitRectangle.data, hitPosition.data, hitOrientation.data
                        )
                    ) {
                        var hurtInventory = registry.GetComponentUnsafe<Comps.Inventory>(hurtEntity);

                        bool isInvulnerable = true; // TODO
                        if (isInvulnerable)
                        {
                            // Deal damage
                            hurtInventory.data[Comps.Item.Kind.Heart].Count--;

                            // TODO: give invulnerability ticks

                            registry.AssignComponent(
                                hurtEntity,
                                new Comps.DamagedByPlayer
                                {
                                    damage = hitDamage.damage,
                                    damagerId = hitEntity.id
                                }
                            );

                            Console.WriteLine(
                                "Entity {0} now at {1} HP (damaged by Entity {2})",
                                hurtEntity.id,
                                hurtInventory.data[Comps.Item.Kind.Heart].Count,
                                hitDamage.attackerId
                            );
                        }
                    }
                }
            }
        }
    }
}
