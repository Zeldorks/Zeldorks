using NetGameShared.Ecs;
using Ecs = NetGameShared.Ecs;
using Comps = NetGameShared.Ecs.Components;

namespace NetGameServer.GameNS.WorldNS.EcsExt.Systems
{
    public static class Item
    {
        private static void SetAttackCharacter(
            Entity owner,
            Ecs.Registry registry
        ) {
            var characterCompOpt = registry.GetComponent<Comps.Character>(owner);
            characterCompOpt.MatchSome(characterComp => {
                characterComp.attacking = true;
                characterComp.attackStartTick = Global.tick;
            });
        }

        public static void Use(
            Comps.Item.Kind itemKind,
            Comps.Inventory inventoryComp,
            Entity owner,
            Ecs.Registry registry,
            Map map
        ) {
            switch (itemKind) {
                case Comps.Item.Kind.Bow:
                    if (inventoryComp.data[Comps.Item.Kind.Rupee].Count > 0) {
                        // Create arrow projectile
                        Factories.Projectile.Create(
                            Comps.Projectile.Kind.Arrow,
                            owner,
                            registry
                        );
                        // Use Rupee
                        inventoryComp.data[Comps.Item.Kind.Rupee].Count--;
                    }
                    break;
                case Comps.Item.Kind.Sword:
                    // Create sword slash
                    Factories.Projectile.Create(
                        Comps.Projectile.Kind.SwordSlash,
                        owner,
                        registry
                    );
                    // Give the character an attack appearance when using the sword
                    SetAttackCharacter(owner, registry);
                    break;
                case Comps.Item.Kind.Bomb:
                    // Create bomb projectile
                    Factories.Projectile.Create(
                        Comps.Projectile.Kind.Bomb,
                        owner,
                        registry
                    );
                    break;
                case Comps.Item.Kind.Boomerang:
                    // Create boomerang projectile
                    Factories.Projectile.Create(
                        Comps.Projectile.Kind.Boomerang,
                        owner,
                        registry
                    );
                    break;
                case Comps.Item.Kind.RedKey:
                    // Create key projectile
                    Factories.Projectile.Create(
                        Comps.Projectile.Kind.RedKey,
                        owner,
                        registry
                    );
                    break;
                case Comps.Item.Kind.BlueKey:
                    // Create key projectile
                    Factories.Projectile.Create(
                        Comps.Projectile.Kind.BlueKey,
                        owner,
                        registry
                    );
                    break;
                case Comps.Item.Kind.GreenKey:
                    // Create key projectile
                    Factories.Projectile.Create(
                        Comps.Projectile.Kind.GreenKey,
                        owner,
                        registry
                    );
                    break;
                case Comps.Item.Kind.PurpleKey:
                    // Create key projectile
                    Factories.Projectile.Create(
                        Comps.Projectile.Kind.PurpleKey,
                        owner,
                        registry
                    );
                    break;
                case Comps.Item.Kind.YellowKey:
                    // Create key projectile
                    Factories.Projectile.Create(
                        Comps.Projectile.Kind.YellowKey,
                        owner,
                        registry
                    );
                    break;
                case Comps.Item.Kind.TriforceShard:
                    // Teleport owner to triforce destination
                    var positionCompOpt = registry
                        .GetComponent<Comps.Position>(owner);

                    positionCompOpt.MatchSome(positionComp => {
                        positionComp.data = map.teleports.triforceDestination;
                    });
                    break;
                default:
                    System.Console.WriteLine(
                        "Item {0} was used, but it did nothing",
                        itemKind
                    );
                    break;

            }
        }
    }
}
