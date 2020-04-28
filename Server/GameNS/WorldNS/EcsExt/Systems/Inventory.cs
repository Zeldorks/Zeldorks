using System.Collections.Generic;
using Optional;

using NetGameShared.Ecs;
using Ecs = NetGameShared.Ecs;
using Comps = NetGameShared.Ecs.Components;

using static NetGameShared.Ecs.Components.Inventory;
using static NetGameShared.Util.RandomExt;

namespace NetGameServer.GameNS.WorldNS.EcsExt.Systems
{
    public static class Inventory
    {
        private static bool ShouldRemoveAfterUse(Comps.Item.Kind itemKind)
        {
            switch (itemKind) {
                case Comps.Item.Kind.Bow:
                case Comps.Item.Kind.Sword:
                case Comps.Item.Kind.RedKey:
                case Comps.Item.Kind.BlueKey:
                case Comps.Item.Kind.GreenKey:
                case Comps.Item.Kind.PurpleKey:
                case Comps.Item.Kind.YellowKey:
                    return false;
                default:
                    return true;
            }
        }

        // Pre-condition: `this.slots` has index `slot`
        public static void UseSlot(
            Comps.Inventory inventoryComp,
            Entity entity,
            int slot,
            Registry registry,
            Map map
        ) {
            Option<Comps.Item.Kind> itemKindOpt = inventoryComp.slots[slot];
            itemKindOpt.Match(
                some: itemKind => {
                    Item.Use(itemKind, inventoryComp, entity, registry, map);
                    if (ShouldRemoveAfterUse(itemKind)) {
                        inventoryComp.Remove(itemKind);
                    }
                },
                none: () => System.Console.WriteLine(
                    "Trying to use empty slot {0}",
                    slot
                )
            );
        }

        private static void Drop(
            Comps.Inventory inventoryComp,
            Comps.Position positionComp,
            Ecs.Registry registry
        ) {
            foreach (KeyValuePair<Comps.Item.Kind, StorageInfo> pair in inventoryComp.data) {
                Comps.Item.Kind itemKind = pair.Key;
                StorageInfo storageInfo = pair.Value;

                for (int i = 0; i < storageInfo.Count; i++) {
                    Entity itemEntity = Factories.Item.Create(
                        itemKind,
                        positionComp.data,
                        registry
                    );

                    registry.AssignComponent(
                        itemEntity,
                        new Comps.Velocity {
                            data = 20 * Global.random.NextPhysicalVector2(),
                            decay = 0.8f
                        }
                    );
                }
            }
        }

        public static void Drop(
            Entity entity,
            Ecs.Registry registry
        ) {
            var inventoryCompOpt = registry
                .GetComponent<Comps.Inventory>(entity);

            var positionCompOpt = registry
                .GetComponent<Comps.Position>(entity);

            inventoryCompOpt.MatchSome(
                inventoryComp => {
                    positionCompOpt.MatchSome(
                        positionComp => Drop(
                            inventoryComp,
                            positionComp,
                            registry
                        )
                    );
                }
            );
        }
    }
}
