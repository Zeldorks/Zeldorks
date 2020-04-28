using System;
using System.Collections.Generic;

using NetGameShared.Ecs;
using Comps = NetGameShared.Ecs.Components;

using Orientation = NetGameShared.Util.Physical.Orientation;
using static NetGameShared.Util.Physical.Orientation.CardinalExt;
using Microsoft.Xna.Framework;
using PhysicalVector2 = Microsoft.Xna.Framework.Vector2;
using Tick = System.UInt32;

namespace NetGameServer.GameNS.WorldNS.EcsExt.Systems
{
    public static class Autonomous
    {
        private static Orientation.Cardinal GetCardinal(int n)
        {
            return (Orientation.Cardinal)(n % 4);
        }

        private static void CreateSpreadshots(
            Entity owner,
            Comps.Autonomous autonomousComp,
            Entity centerProjectile,
            Registry registry
        ) {
            var centerVelocityComp = registry
                .GetComponentUnsafe<Comps.Velocity>(centerProjectile);

            PhysicalVector2 centerVelocity = centerVelocityComp.data;

            List<PhysicalVector2> velocities = new List<PhysicalVector2> {
                PhysicalVector2.Transform(
                    centerVelocity,
                    Matrix.CreateRotationZ((float)Math.PI / 6)
                ),
                PhysicalVector2.Transform(
                    centerVelocity,
                    Matrix.CreateRotationZ(-(float)Math.PI / 6)
                )
            };

            foreach (PhysicalVector2 velocity in velocities) {
                var projectile = Factories.Projectile.Create(
                    autonomousComp.attackType,
                    owner,
                    registry
                );

                var velocityComp = registry
                    .GetComponentUnsafe<Comps.Velocity>(projectile);

                velocityComp.data = velocity;

                // TODO: Make this less hacky.
                //
                // To prevent the spreadshots from immediately colliding with
                // each other, give them some position offset

                var positionComp = registry
                    .GetComponentUnsafe<Comps.Position>(projectile);

                PhysicalVector2 offset = velocity * 16.0f;
                positionComp.data += offset;
            }
        }

        public static void Run(Registry registry)
        {
            HashSet<Entity> entities = registry.GetEntities(
                typeof(Comps.Velocity),
                typeof(Comps.Orientations.Cardinal),
                typeof(Comps.Autonomous)
            );

            Tick tick = Global.tick;
            Random random = Global.random;

            foreach (Entity entity in entities)
            {
                Comps.Autonomous autonomous = registry
                    .GetComponentUnsafe<Comps.Autonomous>(entity);

                Comps.Velocity velocity = registry
                    .GetComponentUnsafe<Comps.Velocity>(entity);

                Comps.Orientations.Cardinal orientation = registry
                    .GetComponentUnsafe<Comps.Orientations.Cardinal>(entity);

                float moveSpeed = autonomous.moveSpeed;
                float changePeriod = 60 * autonomous.changePeriod;

                if (tick % changePeriod == 0)
                {
                    if (autonomous.attackChance > random.NextDouble())
                    {
                        var p = Factories.Projectile.Create(autonomous.attackType, entity, registry);

                        if (autonomous.spreadShots) {
                            CreateSpreadshots(entity, autonomous, p, registry);
                        }

                        if (autonomous.stopWhileAttacking)
                        {
                            velocity.data = PhysicalVector2.Zero;
                            continue;
                        }
                    }
                }

                switch (autonomous.kind)
                {
                    case Comps.Autonomous.Kind.Basic:
                        if (tick % changePeriod == 0)
                        {
                            int r = random.Next(4);
                            ChangeVelocity(velocity, GetCardinal(r), moveSpeed, orientation);
                        }
                        break;
                    case Comps.Autonomous.Kind.Charging:
                        if (tick % (2 * changePeriod) == 0)
                        {
                            int r = 1 + 2*random.Next(2);
                            moveSpeed *= 1.5f;
                            ChangeVelocity(velocity, GetCardinal(r), moveSpeed, orientation);
                        }
                        else if (tick % changePeriod == 0)
                        {
                            int r = 2 * random.Next(2);
                            moveSpeed *= 0.5f;
                            ChangeVelocity(velocity, GetCardinal(r), moveSpeed);
                        }
                        break;
                    case Comps.Autonomous.Kind.Flying:
                        moveSpeed *= (float)(1 + Math.Sin((tick / (changePeriod * 5)) * 2 * Math.PI));
                        if (tick % changePeriod == 0)
                        {
                            int r = random.Next(4);
                            ChangeVelocity(velocity, GetCardinal(r), moveSpeed, orientation);
                        }
                        else
                        {
                            ChangeVelocity(velocity, orientation.data, moveSpeed);
                        }
                        break;
                    case Comps.Autonomous.Kind.Shuffling:
                        if (tick % changePeriod == 0)
                        {
                            if (velocity.data.Length() == 0)
                            {
                                ChangeVelocity(velocity, orientation.data, moveSpeed);
                            }
                            else
                            {
                                velocity.data *= -1;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private static void ChangeVelocity(
            Comps.Velocity velocity,
            Orientation.Cardinal direction,
            float moveSpeed
        ) {
            velocity.data = direction.GetPhysicalVector2() * moveSpeed;
            velocity.decay = 1;
        }

        private static void ChangeVelocity(
            Comps.Velocity velocity,
            Orientation.Cardinal direction,
            float moveSpeed,
            Comps.Orientations.Cardinal orientation
        ) {
            velocity.data = direction.GetPhysicalVector2() * moveSpeed;
            orientation.data = direction;
            velocity.decay = 1;
        }
    }
}
