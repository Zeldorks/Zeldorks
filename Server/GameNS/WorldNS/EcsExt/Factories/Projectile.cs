using System;

using Ecs = NetGameShared.Ecs;
using NetGameShared.Ecs;
using Comps = NetGameShared.Ecs.Components;

using Tick = System.UInt32;
using static NetGameShared.Constants.Server.Timing;

using Physical = NetGameShared.Util.Physical;
using NetGameShared.Util.Physical.Orientation;
using PhysicalVector2 = Microsoft.Xna.Framework.Vector2;

namespace NetGameServer.GameNS.WorldNS.EcsExt.Factories
{
    public static class Projectile
    {
        private static PhysicalVector2 GetStartingPosition(
            Physical.PosRectangle playerPosRectangle,
            Physical.Orientation.Cardinal orientation,
            Physical.Rectangle rectangle
        )
        {
            PhysicalVector2 result =
                playerPosRectangle.GetCenteredBound(orientation);

            switch (orientation)
            {
                case Physical.Orientation.Cardinal.Left:
                    result.X -= (rectangle.Width / 2);
                    break;
                case Physical.Orientation.Cardinal.Right:
                    result.X += (rectangle.Width / 2);
                    break;
                case Physical.Orientation.Cardinal.Up:
                    result.Y -= (rectangle.Height / 2);
                    break;
                case Physical.Orientation.Cardinal.Down:
                    result.Y += (rectangle.Height / 2);
                    break;
            }

            return result;
        }

        public static float GetStartingSpeed(
            Comps.Projectile.Kind kind
        )
        {
            switch (kind)
            {
                case Comps.Projectile.Kind.Arrow:
                    return Global.config.ArrowSpeed;
                case Comps.Projectile.Kind.Bomb:
                    return Global.config.BombSpeed;
                case Comps.Projectile.Kind.Boomerang:
                    return Global.config.BoomerangSpeed;
                case Comps.Projectile.Kind.RedKey:
                case Comps.Projectile.Kind.BlueKey:
                case Comps.Projectile.Kind.GreenKey:
                case Comps.Projectile.Kind.PurpleKey:
                case Comps.Projectile.Kind.YellowKey:
                    return Global.config.KeySpeed;
                case Comps.Projectile.Kind.SwordSlash:
                    return Global.config.SwordSlashSpeed;
                case Comps.Projectile.Kind.Fireball:
                    return Global.config.FireballSpeed;
                default:
                    throw new NotImplementedException();
            }
        }

        private static bool IsDamaging(
            Comps.Projectile.Kind kind
        ) {
            switch (kind) {
                case Comps.Projectile.Kind.RedKey:
                case Comps.Projectile.Kind.BlueKey:
                case Comps.Projectile.Kind.GreenKey:
                case Comps.Projectile.Kind.PurpleKey:
                case Comps.Projectile.Kind.YellowKey:
                    return false;
                default:
                    return true;
            }
        }

        private static Tick GetLifetime(
            Comps.Projectile.Kind kind
        ) {
            switch (kind) {
                case Comps.Projectile.Kind.RedKey:
                case Comps.Projectile.Kind.BlueKey:
                case Comps.Projectile.Kind.GreenKey:
                case Comps.Projectile.Kind.PurpleKey:
                case Comps.Projectile.Kind.YellowKey:
                case Comps.Projectile.Kind.SwordSlash:
                    return 1;
                case Comps.Projectile.Kind.Boomerang:
                    return 200;
                default:
                    return ToTicks(TimeSpan.FromSeconds(2));
            }
        }

        public static Entity Create(
            Comps.Projectile.Kind kind,
            Entity owner,
            Ecs.Registry registry
        )
        {
            // Assume that `owner` has position and orientation
            // components

            var playerPositionComp = registry
                .GetComponentUnsafe<Comps.Position>(owner);

            var playerRectangleComp = registry
                .GetComponentUnsafe<Comps.Shapes.Rectangle>(owner);

            var playerOrientationComp = registry
                .GetComponentUnsafe<Comps.Orientations.Cardinal>(owner);

            var playerPosRectangle = new Physical.PosRectangle(
                playerPositionComp.data,
                playerRectangleComp.data
            );

            // Create projectile
            Physical.Rectangle rectangle;
            switch (kind) {
                case Comps.Projectile.Kind.Arrow: 
                    rectangle = new Physical.Rectangle(
                        16.0f * 4,
                        5.0f * 4
                    );
                    break;
                case Comps.Projectile.Kind.SilverArrow:
                    rectangle = new Physical.Rectangle(
                        16.0f * 4,
                        5.0f * 4
                    );
                    break;
                case Comps.Projectile.Kind.Bomb:
                    rectangle = new Physical.Rectangle(
                        14.0f * 4,
                        8.0f * 4
                    );
                    break;
                case Comps.Projectile.Kind.Boomerang:
                    rectangle = new Physical.Rectangle(
                        8.0f * 4,
                        5.0f * 4
                    );
                    break;
                case Comps.Projectile.Kind.MagicalBoomerang:
                    rectangle = new Physical.Rectangle(
                        8.0f * 4,
                        5.0f * 4
                    );
                    break;
                case Comps.Projectile.Kind.Sword:
                    rectangle = new Physical.Rectangle(
                        16.0f * 4,
                        7.0f * 4
                    );
                    break;
                case Comps.Projectile.Kind.WhiteSword:
                    rectangle = new Physical.Rectangle(
                        16.0f * 4,
                        7.0f * 4
                    );
                    break;
                default:
                    rectangle = new Physical.Rectangle(
                        16.0f * 4,
                        16.0f * 4
                    );
                    break;
            }   
            PhysicalVector2 position = GetStartingPosition(
                playerPosRectangle,
                playerOrientationComp.data,
                rectangle
            );

            var projectileEntity = registry.CreateEntity();

            registry.AssignComponent(
                projectileEntity,
                new Comps.Shapes.Rectangle
                {
                    data = rectangle
                }
            );

            registry.AssignComponent(
                projectileEntity,
                new Comps.Position { data = position }
            );

            registry.AssignComponent(
                projectileEntity,
                (Comps.Orientations.Cardinal)playerOrientationComp.Clone()
            );

            PhysicalVector2 velocity =
                playerOrientationComp.data.GetPhysicalVector2() *
                GetStartingSpeed(kind);
            
            if (kind != Comps.Projectile.Kind.Boomerang)
            {
                registry.AssignComponent(
               projectileEntity,
               new Comps.Velocity
               {
                   data = velocity,
                   decay = 1.0f
               }
               );

            }
            else
            {
                registry.AssignComponent(
               projectileEntity,
               new Comps.Velocity
               {
                   data = velocity,
                   decay = .95f
               }
               );
            
            registry.AssignComponent(
               projectileEntity,
               new Comps.ReturnVelocity
               {
                   data = velocity,
                   decay = .95f
               }
               );

            }
            registry.AssignComponent(
                projectileEntity,
                new Comps.Projectile
                {
                    kind = kind,
                    ownerId = owner.id
                }
            );

            if (IsDamaging(kind)) {
                registry.AssignComponent(
                    projectileEntity,
                    new Comps.Damaging {
                        damage = 1,
                        attackerId = owner.id
                    }
                );
            }

            registry.AssignComponent(
                projectileEntity,
                new Comps.Lifetime {
                    ticks = GetLifetime(kind)
                }
            );

            registry.AssignComponent(
                projectileEntity,
                new Comps.Solid { ephemeral = true }
            );
            return projectileEntity;
        }
        }
    }

