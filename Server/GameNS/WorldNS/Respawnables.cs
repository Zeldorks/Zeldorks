using System;
using Optional;
using System.Collections.Generic;

using NetGameShared.Ecs;
using static NetGameShared.Ecs.EntityOption;
using Ecs = NetGameShared.Ecs;
using Comps = NetGameShared.Ecs.Components;
using NetGameServer.GameNS.WorldNS.MapsExt;

using static NetGameShared.Constants.Server.Timing;

using PhysicalVector2 = Microsoft.Xna.Framework.Vector2;
using Tick = System.UInt32;

namespace NetGameServer.GameNS.WorldNS
{
    public class Respawnable
    {
        public Option<Entity> entityOpt;
        public Spawns.Kind spawnKind;
        public PhysicalVector2 position;
        public Tick respawnDelay;
        public Tick despawnStartTime;
    }

    public class Respawnables
    {
        private List<Respawnable> data;

        // Depedencies
        private Ecs.Registry registry;
        private Map map;

        private static bool ShouldSpawn(Spawns.Kind spawnKind)
        {
            switch (spawnKind) {
                case Spawns.Kind.Player:
                    return false;
                default:
                    return true;
            }
        }

        private static bool IsRespawnable(Spawns.Kind spawnKind)
        {
            switch (spawnKind) {
                case Spawns.Kind.ItemRedKey:
                case Spawns.Kind.ItemBlueKey:
                case Spawns.Kind.ItemGreenKey:
                case Spawns.Kind.ItemPurpleKey:
                case Spawns.Kind.ItemYellowKey:
                    return false;
                default:
                    return true;
            }
        }

        public void Register(Respawnable respawnable)
        {
            data.Add(respawnable);
        }

        private Entity Spawn(
            Spawns.Kind kind,
            PhysicalVector2 position
        ) {
            switch (kind) {
                case Spawns.Kind.ItemCompass:
                    return EcsExt.Factories.Item.Create(
                        Comps.Item.Kind.Compass,
                        position,
                        registry
                    );
                case Spawns.Kind.ItemHeart:
                    return EcsExt.Factories.Item.Create(
                        Comps.Item.Kind.Heart,
                        position,
                        registry
                    );
                case Spawns.Kind.ItemBomb:
                    return EcsExt.Factories.Item.Create(
                        Comps.Item.Kind.Bomb,
                        position,
                        registry
                    );
                case Spawns.Kind.ItemBoomerang:
                    return EcsExt.Factories.Item.Create(
                        Comps.Item.Kind.Boomerang,
                        position,
                        registry
                    );
                case Spawns.Kind.ItemClock:
                    return EcsExt.Factories.Item.Create(
                        Comps.Item.Kind.Clock,
                        position,
                        registry
                    );
                case Spawns.Kind.ItemBow:
                    return EcsExt.Factories.Item.Create(
                        Comps.Item.Kind.Bow,
                        position,
                        registry
                    );
                case Spawns.Kind.ItemSword:
                    return EcsExt.Factories.Item.Create(
                        Comps.Item.Kind.Sword,
                        position,
                        registry
                    );
                case Spawns.Kind.ItemRupee:
                    return EcsExt.Factories.Item.Create(
                        Comps.Item.Kind.Rupee,
                        position,
                        registry
                    );
                case Spawns.Kind.ItemTriforceShard:
                    return EcsExt.Factories.Item.Create(
                        Comps.Item.Kind.TriforceShard,
                        position,
                        registry
                    );
                case Spawns.Kind.ItemCandle:
                    return EcsExt.Factories.Item.Create(
                        Comps.Item.Kind.Candle,
                        position,
                        registry
                    );
                case Spawns.Kind.ItemMap:
                    return EcsExt.Factories.Item.Create(
                        Comps.Item.Kind.Map,
                        position,
                        registry
                    );
                case Spawns.Kind.ItemRedKey:
                    return EcsExt.Factories.Item.Create(
                        Comps.Item.Kind.RedKey,
                        position,
                        registry
                    );
                case Spawns.Kind.ItemBlueKey:
                    return EcsExt.Factories.Item.Create(
                        Comps.Item.Kind.BlueKey,
                        position,
                        registry
                    );
                case Spawns.Kind.ItemGreenKey:
                    return EcsExt.Factories.Item.Create(
                        Comps.Item.Kind.GreenKey,
                        position,
                        registry
                    );
                case Spawns.Kind.ItemPurpleKey:
                    return EcsExt.Factories.Item.Create(
                        Comps.Item.Kind.PurpleKey,
                        position,
                        registry
                    );
                case Spawns.Kind.ItemYellowKey:
                    return EcsExt.Factories.Item.Create(
                        Comps.Item.Kind.YellowKey,
                        position,
                        registry
                    );

                case Spawns.Kind.ObstacleRedDoor:
                    return EcsExt.Factories.Door.Create(
                        Comps.Door.Kind.Red,
                        position,
                        registry
                    );
                case Spawns.Kind.ObstacleBlueDoor:
                    return EcsExt.Factories.Door.Create(
                        Comps.Door.Kind.Blue,
                        position,
                        registry
                    );
                case Spawns.Kind.ObstacleGreenDoor:
                    return EcsExt.Factories.Door.Create(
                        Comps.Door.Kind.Green,
                        position,
                        registry
                    );
                case Spawns.Kind.ObstaclePurpleDoor:
                    return EcsExt.Factories.Door.Create(
                        Comps.Door.Kind.Purple,
                        position,
                        registry
                    );
                case Spawns.Kind.ObstacleYellowDoor:
                    return EcsExt.Factories.Door.Create(
                        Comps.Door.Kind.Yellow,
                        position,
                        registry
                    );
                case Spawns.Kind.EnemyAquamentus:
                    return EcsExt.Factories.Character.CreateEnemy(
                        Comps.Character.Kind.Aquamentus,
                        position,
                        registry
                    );
                case Spawns.Kind.EnemyZol:
                    return EcsExt.Factories.Character.CreateEnemy(
                        Comps.Character.Kind.Zol,
                        position,
                        registry
                    );
                case Spawns.Kind.EnemyGoriya:
                    return EcsExt.Factories.Character.CreateEnemy(
                        Comps.Character.Kind.Goriya,
                        position,
                        registry
                    );
                case Spawns.Kind.EnemyDodongos:
                    return EcsExt.Factories.Character.CreateEnemy(
                        Comps.Character.Kind.Dodongos,
                        position,
                        registry
                    );
                case Spawns.Kind.EnemyStalfos:
                    return EcsExt.Factories.Character.CreateEnemy(
                        Comps.Character.Kind.Stalfos,
                        position,
                        registry
                    );
                case Spawns.Kind.EnemyRope:
                    return EcsExt.Factories.Character.CreateEnemy(
                        Comps.Character.Kind.Rope,
                        position,
                        registry
                    );
                case Spawns.Kind.EnemyKeese:
                    return EcsExt.Factories.Character.CreateEnemy(
                        Comps.Character.Kind.Keese,
                        position,
                        registry
                    );
                case Spawns.Kind.EnemyWallmaster:
                    return EcsExt.Factories.Character.CreateEnemy(
                        Comps.Character.Kind.Wallmaster,
                        position,
                        registry
                    );
                default:
                    throw new ArgumentException("Invalid spawning kind");
            }
        }

        private static TimeSpan GetDelay(Spawns.Kind kind)
        {
            switch (kind) {
                case Spawns.Kind.ItemCompass:
                case Spawns.Kind.ItemHeart:
                case Spawns.Kind.ItemBomb:
                case Spawns.Kind.ItemBoomerang:
                case Spawns.Kind.ItemClock:
                case Spawns.Kind.ItemBow:
                case Spawns.Kind.ItemSword:
                case Spawns.Kind.ItemRupee:
                case Spawns.Kind.ItemTriforceShard:
                case Spawns.Kind.ItemCandle:
                case Spawns.Kind.ItemMap:
                case Spawns.Kind.ItemBlueKey:
                case Spawns.Kind.ItemRedKey:
                case Spawns.Kind.ItemGreenKey:
                case Spawns.Kind.ItemPurpleKey:
                case Spawns.Kind.ItemYellowKey:
                    return TimeSpan.FromSeconds(3);
                case Spawns.Kind.EnemyAquamentus:
                case Spawns.Kind.EnemyZol:
                case Spawns.Kind.EnemyGoriya:
                case Spawns.Kind.EnemyDodongos:
                case Spawns.Kind.EnemyStalfos:
                case Spawns.Kind.EnemyRope:
                case Spawns.Kind.EnemyKeese:
                case Spawns.Kind.EnemyWallmaster:
                    return TimeSpan.FromSeconds(5);
                case Spawns.Kind.ObstacleBlueDoor:
                case Spawns.Kind.ObstacleRedDoor:
                case Spawns.Kind.ObstacleGreenDoor:
                case Spawns.Kind.ObstaclePurpleDoor:
                case Spawns.Kind.ObstacleYellowDoor:
                    return TimeSpan.FromSeconds(10);
                default:
                    throw new ArgumentException("Invalid spawning kind");
            }
        }

        private void Initialize()
        {
            // Spawn intial entities and register each
            foreach (
                KeyValuePair<Spawns.Kind, List<PhysicalVector2>> pair in
                map.spawns.spawnPositions
            ) {
                Spawns.Kind kind = pair.Key;
                if (!ShouldSpawn(kind)) continue;

                List<PhysicalVector2> positions = pair.Value;

                foreach (PhysicalVector2 position in positions) {
                    Entity entity = Spawn(kind, position);

                    var respawnable = new Respawnable {
                        entityOpt = entity.Some(),
                        spawnKind = kind,
                        position = position,
                        respawnDelay = ToTicks(GetDelay(kind))
                    };

                    if (IsRespawnable(kind)) {
                        Register(respawnable);
                    }
                }
            }
        }

        public Respawnables(Registry registry, Map map)
        {
            this.registry = registry;
            this.map = map;
            data = new List<Respawnable>();
            Initialize();
        }

        // Assume that all entites have been removed prior to calling this
        public void Reset()
        {
            data.Clear();
            Initialize();
        }

        public void Update()
        {
            for (int i = 0; i < data.Count; i++)
            {
                Respawnable respawnable = data[i];
                switch (respawnable.entityOpt.Update(registry)) {
                    case EntityOption.State.JustRemoved:
                        respawnable.despawnStartTime = Global.tick;
                        break;
                    case EntityOption.State.Removed:
                        Tick elapsed = Global.tick - respawnable.despawnStartTime;
                        if (elapsed > respawnable.respawnDelay) {
                            respawnable.entityOpt = Spawn(
                                respawnable.spawnKind,
                                respawnable.position
                            ).Some();
                        }
                        break;
                }
            }
        }
    }
}
