using System.Collections.Generic;
using Optional;

using GameInputs = NetGameShared.Net.Protocol.GameInputs;

using Ecs = NetGameShared.Ecs;
using NetGameShared.Ecs;
using static NetGameShared.Ecs.EntityOption;
using EcsExt = NetGameServer.GameNS.WorldNS.EcsExt;
using Comps = NetGameShared.Ecs.Components;

using NetGameServer.GameNS.WorldNS.MapsExt;

using ClientId = System.Int32;

namespace NetGameServer.GameNS
{
    public class Player
    {
        private ClientId clientId;
        private Option<Ecs.Entity> entityOpt;
        public int score;

        // Dependencies
        private World world; // World the player exists in
        private Controller controller;

        public Player(ClientId clientId, World world, Controller controller)
        {
            this.clientId = clientId;
            this.world = world;
            this.controller = controller;
        }

        public void ProcessMovement(GameInputs.Movement movement)
        {
            if (movement == GameInputs.Movement.None) return;

            entityOpt.Update(
                world.ecsRegistry,
                entity => {
                    Ecs.Systems.Movement.Process(
                        movement,
                        entity,
                        world.ecsRegistry
                    );
                    Ecs.Systems.CharacterOrientation.Process(
                        movement,
                        entity,
                        world.ecsRegistry
                    );
                    Ecs.Systems.CharacterAttack.Process(
                        movement,
                        entity,
                        world.ecsRegistry
                    );
                }
            );
        }

        // If `forward` is true, load the item into `slot` with the next item.
        // If `forward` is false, load the previous one.
        public void LoadItemInSlot(int slot, bool forward)
        {
            entityOpt.Update(
                world.ecsRegistry,
                entity => {
                    // Assume player has an inventory
                    var inventoryComp = world.ecsRegistry
                        .GetComponentUnsafe<Comps.Inventory>(entity);

                    inventoryComp.LoadItemInSlot(slot, forward);
                }
            );
        }

        public void UseSlots(HashSet<int> slots)
        {
            if (slots.Count > 0) {
                entityOpt.Update(
                    world.ecsRegistry,
                    entity => {
                        // Assume player has an inventory
                        var inventoryComp = world.ecsRegistry
                            .GetComponentUnsafe<Comps.Inventory>(entity);

                        foreach (int slot in slots) {
                            EcsExt.Systems.Inventory.UseSlot(
                                inventoryComp,
                                entity,
                                slot,
                                world.ecsRegistry,
                                world.map
                            );
                        }
                    }
                );
            }
        }

        private Comps.Character.Kind GetRandomCharacterKind()
        {
            Comps.Character.Kind start;
            Comps.Character.Kind end;
            (start, end) = Comps.Character.KindRanges.players;

            // Random.Next(start, end) exludes `end`, so we add 1 to include it
            return (Comps.Character.Kind)Global.random.Next((int)start, (int)end + 1);
        }

        public void Spawn()
        {
            this.entityOpt = EcsExt.Factories.Character.CreatePlayer(
                GetRandomCharacterKind(),
                world.map.spawns.GetSpawnPosition(Spawns.Kind.Player),
                clientId,
                world.ecsRegistry
            ).Some();
        }

        public void Update()
        {
            switch (entityOpt.Update(world.ecsRegistry)) {
                case EntityOption.State.JustRemoved:
                case EntityOption.State.Removed:
                    if (controller.CanSpawn()) {
                        Spawn();
                    }
                    break;
            }
        }

        // Client is disconnecting and player is about to be deleted
        public void ProcessDisconnect()
        {
            entityOpt.Update(
                world.ecsRegistry,
                entity => world.ecsRegistry.Remove(entity)
            );
        }
    }
}
