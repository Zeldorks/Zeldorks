using System.Collections.Generic;
using NetGameShared.Ecs;
using Comps = NetGameShared.Ecs.Components;
using ClientId = System.Int32;
using Optional.Unsafe;

namespace NetGameServer.GameNS.WorldNS.EcsExt.Systems
{
    public static class Death
    {
        public static void Kill(
            Entity victim,
            Entity damager,
            Registry registry,
            Game game
        ) {
            // Note: `damager` is not necessarily the `killer`. For example, a
            // projectile is the damager, but the killer is the owner of the
            // projectile.

            var projectileCompOpt = registry
                .GetComponent<Comps.Projectile>(damager);

            if (projectileCompOpt.HasValue) {
                var projectileComp = projectileCompOpt.ValueOrFailure();
                Entity owner = new Entity(projectileComp.ownerId);
                Kill(victim, owner, registry, game);
                return;
            }

            // ---

            var killerPlayableCompOpt = registry
                .GetComponent<Comps.Playable>(damager);

            var victimPlayableCompOpt = registry
                .GetComponent<Comps.Playable>(victim);

            var killerCharacterCompOpt = registry
                .GetComponent<Comps.Character>(damager);

            var victimCharacterCompOpt = registry
                .GetComponent<Comps.Character>(victim);

            // Announce kill

            if (killerPlayableCompOpt.HasValue && victimPlayableCompOpt.HasValue) {
                // Player killed player
                var killerPlayableComp = killerPlayableCompOpt.ValueOrFailure();
                var victimPlayableComp = victimPlayableCompOpt.ValueOrFailure();

                ClientId killerClientId = killerPlayableComp.clientId;
                ClientId victimClientId = victimPlayableComp.clientId;

                System.Console.WriteLine(
                    "Player {0} killed player {1}",
                    killerClientId,
                    victimClientId
                );
            } else if (killerPlayableCompOpt.HasValue && victimCharacterCompOpt.HasValue) {
                // Player killed enemy
                var killerPlayableComp = killerPlayableCompOpt.ValueOrFailure();
                var victimCharacterComp = victimCharacterCompOpt.ValueOrFailure();

                ClientId killerClientId = killerPlayableComp.clientId;
                Comps.Character.Kind victimCharacterKind = victimCharacterComp.kind;

                System.Console.WriteLine(
                    "Player {0} killed a {1}",
                    killerClientId,
                    victimCharacterKind
                );
            } else if (killerCharacterCompOpt.HasValue && victimPlayableCompOpt.HasValue) {
                // Enemy killed player
                var killerCharacterComp = killerCharacterCompOpt.ValueOrFailure();
                var victimPlayableComp = victimPlayableCompOpt.ValueOrFailure();

                Comps.Character.Kind killerCharacterKind = killerCharacterComp.kind;
                ClientId victimClientId = victimPlayableComp.clientId;

                System.Console.WriteLine(
                    "Enemy {0} killed player {1}",
                    killerCharacterKind,
                    victimClientId
                );
            } else if (killerCharacterCompOpt.HasValue && victimCharacterCompOpt.HasValue) {
                // Enemy killed enemy
                var killerCharacterComp = killerCharacterCompOpt.ValueOrFailure();
                var victimCharacterComp = victimCharacterCompOpt.ValueOrFailure();

                Comps.Character.Kind killerCharacterKind = killerCharacterComp.kind;
                Comps.Character.Kind victimCharacterKind = victimCharacterComp.kind;

                System.Console.WriteLine(
                    "Enemy {0} killed enemy {1}",
                    killerCharacterKind,
                    victimCharacterKind
                );
            }

            // Scoring

            if (killerPlayableCompOpt.HasValue) {
                var killerPlayableComp = killerPlayableCompOpt.ValueOrFailure();
                ClientId killerClientId = killerPlayableComp.clientId;

                Player killer = game.players[killerClientId];
                killer.score++;

                System.Console.WriteLine(
                    "Player {0} now has a score of {1}",
                    killerClientId,
                    killer.score
                );
            }

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
                typeof(Comps.DamagedByPlayer)
            );

            // Queue entities to remove so we aren't removing entities while
            // iterating over them
            //
            // Item1: victim
            // Item2: damager
            Queue<(Entity, Entity)> toKill = new Queue<(Entity, Entity)>();

            foreach (Entity entity in entities) {
                var damagedComp = registry.GetComponentUnsafe<Comps.DamagedByPlayer>(entity);

                // Assume that all entities with a `Damaged` component also has
                // an `Inventory` component
                var inventoryComp = registry.GetComponentUnsafe<Comps.Inventory>(entity);

                if (inventoryComp.data[Comps.Item.Kind.Heart].Count <= 0) {
                    var damager = new Entity(damagedComp.damagerId);
                    toKill.Enqueue((entity, damager));
                }
            }

            ProcessKills(toKill, registry, game);
        }
    }
}
