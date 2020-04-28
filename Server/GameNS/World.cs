using NetGameServer.GameNS.WorldNS;
using Ecs = NetGameShared.Ecs;
using EcsExt = NetGameServer.GameNS.WorldNS.EcsExt;
using Comps = NetGameShared.Ecs.Components;

namespace NetGameServer.GameNS
{
    public class World
    {
        public EcsExt.Registry ecsRegistry;
        public Map map;

        public Respawnables respawnables;

        // Dependencies
        private Game game; // The game that the world belongs to

        public World(Game game)
        {
            this.game = game;
            ecsRegistry = new EcsExt.Registry();
            map = new Map("Adventure");
            respawnables = new Respawnables(ecsRegistry, map);
        }

        public void Update()
        {
            Ecs.Systems.Lifetime.Run(ecsRegistry);
            respawnables.Update();

            Ecs.Systems.CharacterAttack.Run(ecsRegistry, Global.tick);

            Ecs.Systems.Movement.Run(ecsRegistry);

            EcsExt.Systems.Autonomous.Run(ecsRegistry);

            // `Collision.Items` must run before `Collision.SolidToSolid`
            Ecs.Systems.Collision.Items.Run(ecsRegistry);

            // `Collision.DamageDetection` must run before `SolidToSolid`
            Ecs.Systems.Collision.DamageDetection.Run(ecsRegistry);

            Ecs.Systems.Collision.UnlockDoor.Run(ecsRegistry);

            Ecs.Systems.Collision.EnemyHitsPlayer.Run(ecsRegistry);

            // Process damage
            EcsExt.Systems.Death.Run(ecsRegistry, game);
            EcsExt.Systems.DeathByNPC.Run(ecsRegistry, game);
            ecsRegistry.ClearComponent(typeof(Comps.DamagedByPlayer));

            Ecs.Systems.Collision.SolidToSolid.Run(ecsRegistry);

            // `SolidToMap` must run after `SolidToSolid` to prevent players
            // from phasing into walls
            Ecs.Systems.Collision.SolidToMap.Run(ecsRegistry, map);
        }

        public void Reset()
        {
            ecsRegistry.Clear();
            respawnables.Reset();
        }
    }
}
