using System.Collections.Generic;
using NetGameShared.Ecs;
using Comps = NetGameShared.Ecs.Components;
using ClientId = System.Int32;

namespace NetGameServer.GameNS.WorldNS.EcsExt.Systems
{
    public static class DeathByNPC
    {
        public static void Kill(
            Entity victim,
            Entity damager,
            Registry registry,
            Game game
        ) {
            // Note: `damager` is not necessarily the `killer`. For examples, a
            // projectile is the damager, but the killer is the owner of the
            // projectile.

            var killerPlayableCompOpt = registry
                .GetComponent<Comps.Playable>(damager);

            killerPlayableCompOpt.Match(
                some: killerPlayableComp => {
                    ClientId killerClientId = killerPlayableComp.clientId;
                    Player killer = game.players[killerClientId];
                    

                },
                none: () => {
                    var projectileComp = registry
                        .GetComponentUnsafe<Comps.Projectile>(damager);

                    Entity owner = new Entity(projectileComp.ownerId);
                    Kill(victim, owner, registry, game);
                }
            );

            Inventory.Drop(victim, registry);
            registry.Remove(victim);
        }

        public static void ProcessKills(
            Queue<(Entity, Entity)> toKill,
            Registry registry,
            Game game
        ) {
            while (toKill.Count > 0) {
                (Entity victim, Entity damager) = toKill.Dequeue();
                Kill(victim, damager, registry, game);
            }
        }

        public static void Run(Registry registry, Game game)
        {
            HashSet<Entity> entities = registry.GetEntities(
                typeof(Comps.Playable)
            );

          

            // Queue entities to remove so we aren't removing entities while
            // iterating over them
            //
            // Item1: victim
            // Item2: damager           
            foreach (Entity entity in entities) {
               
                var inventoryComp = registry.GetComponentUnsafe<Comps.Inventory>(entity);

                if (inventoryComp.data[Comps.Item.Kind.Heart].Count <= 0) {

                    Inventory.Drop(entity, registry);
                    registry.Remove(entity);
                }
            }

        }
    }
}
