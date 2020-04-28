using System;

using NetGameShared.Ecs;
using Ecs = NetGameShared.Ecs;
using Comps = NetGameShared.Ecs.Components;

using Physical = NetGameShared.Util.Physical;
using PhysicalVector2 = Microsoft.Xna.Framework.Vector2;

using ClientId = System.Int32;

namespace NetGameServer.GameNS.WorldNS.EcsExt.Factories
{
    public static class Character
    {
        private static Physical.Rectangle GetShape(Comps.Character.Kind kind)
        {
            var players = Comps.Character.KindRanges.players;
            switch (kind) {
                case var k when k >= players.start && k <= players.end:
                    return new Physical.Rectangle(64.0f, 64.0f);
                case Comps.Character.Kind.Aquamentus:
                    return new Physical.Rectangle(96.0f, 128.0f);
                case Comps.Character.Kind.Zol:
                    return new Physical.Rectangle(64.0f, 64.0f);
                case Comps.Character.Kind.Goriya:
                    return new Physical.Rectangle(64.0f, 64.0f);
                case Comps.Character.Kind.Dodongos:
                    return new Physical.Rectangle(96.0f, 64.0f);
                case Comps.Character.Kind.Stalfos:
                    return new Physical.Rectangle(64.0f, 64.0f);
                case Comps.Character.Kind.Rope:
                    return new Physical.Rectangle(64.0f, 64.0f);
                case Comps.Character.Kind.Keese:
                    return new Physical.Rectangle(64.0f, 64.0f);
                case Comps.Character.Kind.Wallmaster:
                    return new Physical.Rectangle(64.0f, 64.0f);
                default:
                    throw new System.NotImplementedException();
            }
        }

        private static Comps.Autonomous GetAutonomous(Comps.Character.Kind kind)
        {
            switch(kind)
            {
                case Comps.Character.Kind.Aquamentus:
                    return new Comps.Autonomous() {
                        attackType = Comps.Projectile.Kind.Fireball,
                        attackChance = 0.75f,
                        stopWhileAttacking = false,
                        kind = Comps.Autonomous.Kind.Shuffling,
                        moveSpeed = 0.75f,
                        changePeriod = 1.5f,
                        spreadShots = true
                    };
                case Comps.Character.Kind.Dodongos:
                    return new Comps.Autonomous()
                    {
                        changePeriod = 2,
                        moveSpeed = 0.5f
                    };
                case Comps.Character.Kind.Goriya:
                    return new Comps.Autonomous() {
                        attackType = Comps.Projectile.Kind.Boomerang,
                        attackChance = 0.25f
                    };
                case Comps.Character.Kind.Keese:
                    return new Comps.Autonomous()
                    {
                        kind = Comps.Autonomous.Kind.Flying,
                        changePeriod = 0.5f,
                        moveSpeed = 3
                    };
                case Comps.Character.Kind.Rope:
                    return new Comps.Autonomous()
                    {
                        kind = Comps.Autonomous.Kind.Charging,
                        moveSpeed = 5
                    };
                case Comps.Character.Kind.Stalfos:
                    return new Comps.Autonomous();
                case Comps.Character.Kind.Wallmaster:
                    return new Comps.Autonomous()
                    {
                        moveSpeed = 0.5f
                    };
                case Comps.Character.Kind.Zol:
                    return new Comps.Autonomous()
                    {
                        moveSpeed = 0.75f,
                        changePeriod = 2
                    };
                default:
                    throw new ArgumentException(
                        String.Format("{0} is not a valid enemy kind", kind),
                        "kind"
                    );
            }
        }

        private static Comps.Inventory GetInventory(Comps.Character.Kind kind)
        {
            var result = new Comps.Inventory();
            var players = Comps.Character.KindRanges.players;

            switch (kind) {
                case var k when k >= players.start && k <= players.end:
                case Comps.Character.Kind.Aquamentus:
                    result.data[Comps.Item.Kind.Heart].Count = 6;
                    result.data[Comps.Item.Kind.Rupee].Count = 10;
                    break;
                case Comps.Character.Kind.Wallmaster:
                    result.data[Comps.Item.Kind.Heart].Count = 4;
                    result.data[Comps.Item.Kind.Rupee].Count = 5;
                    break;
                case Comps.Character.Kind.Dodongos:
                case Comps.Character.Kind.Goriya:
                    result.data[Comps.Item.Kind.Heart].Count = 3;
                    result.data[Comps.Item.Kind.Rupee].Count = 3;
                    break;
                case Comps.Character.Kind.Rope:
                    result.data[Comps.Item.Kind.Heart].Count = 2;
                    result.data[Comps.Item.Kind.Rupee].Count = 2;
                    break;
            }

            return result;
        }

        public static Entity Create(
            Comps.Character.Kind kind,
            PhysicalVector2 position,
            Ecs.Registry registry
        ) {
            var result = registry.CreateEntity();

            registry.AssignComponent(
                result,
                new Comps.Position { data = position }
            );

            registry.AssignComponent(
                result,
                new Comps.Orientations.Cardinal {
                    data = Physical.Orientation.Cardinal.Right
                }
            );

            registry.AssignComponent(
                result,
                GetInventory(kind)
            );

            registry.AssignComponent(
                result,
                new Comps.Velocity() { data = PhysicalVector2.Zero }
            );

            registry.AssignComponent(
                result,
                new Comps.Solid()
            );

            registry.AssignComponent(
                result,
                new Comps.Character { kind = kind }
            );

            registry.AssignComponent(
                result,
                new Comps.Shapes.Rectangle { data = GetShape(kind) }
            );

            return result;
        }

        public static Entity CreatePlayer(
            Comps.Character.Kind kind,
            PhysicalVector2 position,
            ClientId clientId,
            Ecs.Registry registry
        ) {
            var result = Create(kind, position, registry);
            registry.AssignComponent(
                result,
                new Comps.Playable {
                    clientId = clientId
                }
            );

            registry.AssignComponent(
                result,
                new Comps.Invulnerability
                {
                    isInvulnerable = false,
                    ticks = 0
                }
            ) ;
            return result;
        }
        public static Entity CreateEnemy(
            Comps.Character.Kind kind,
            PhysicalVector2 position,
            Ecs.Registry registry
        ) {
            var result = Create(kind, position, registry);
            registry.AssignComponent(
                result,
                GetAutonomous(kind)
            );
            registry.AssignComponent(
                result,
                new Comps.Damaging
                {
                    damage = 1,
                    attackerId=0
                }

            );

           
            return result;
        }
    }
}
