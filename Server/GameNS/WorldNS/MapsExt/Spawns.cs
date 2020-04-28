using System.Collections.Generic;
using System.Diagnostics;
using Optional;
using TiledSharp;

using PhysicalVector2 = Microsoft.Xna.Framework.Vector2;

namespace NetGameServer.GameNS.WorldNS.MapsExt
{
    public class Spawns
    {
        // TODO: Add more spawn kinds
        public enum Kind {
            Player,
            EnemyAquamentus,
            EnemyZol,
            EnemyGoriya,
            EnemyDodongos,
            EnemyStalfos,
            EnemyRope,
            EnemyKeese,
            EnemyWallmaster,
            ItemCompass,
            ItemHeart,
            ItemBomb,
            ItemBoomerang,
            ItemClock,
            ItemBow,
            ItemSword,
            ItemRupee,
            ItemTriforceShard,
            ItemCandle,
            ItemMap,
            ItemRedKey,
            ItemBlueKey,
            ItemGreenKey,
            ItemPurpleKey,
            ItemYellowKey,
            ObstacleBlueDoor,
            ObstacleRedDoor,
            ObstacleGreenDoor,
            ObstaclePurpleDoor,
            ObstacleYellowDoor
        }

        public Dictionary<Kind, List<PhysicalVector2>> spawnPositions;

        private Option<Kind> GetKind(string type)
        {
            switch (type) {
                case "Player": return Kind.Player.Some();
                case "ItemCompass": return Kind.ItemCompass.Some();
                case "ItemHeart": return Kind.ItemHeart.Some();
                case "ItemBomb": return Kind.ItemBomb.Some();
                case "ItemBoomerang": return Kind.ItemBoomerang.Some();
                case "ItemClock": return Kind.ItemClock.Some();
                case "ItemBow": return Kind.ItemBow.Some();
                case "ItemSword": return Kind.ItemSword.Some();
                case "ItemRupee": return Kind.ItemRupee.Some();
                case "ItemTriforceShard": return Kind.ItemTriforceShard.Some();
                case "ItemCandle": return Kind.ItemCandle.Some();
                case "ItemMap": return Kind.ItemMap.Some();
                case "ItemRedKey": return Kind.ItemRedKey.Some();
                case "ItemBlueKey": return Kind.ItemBlueKey.Some();
                case "ItemGreenKey": return Kind.ItemGreenKey.Some();
                case "ItemPurpleKey": return Kind.ItemPurpleKey.Some();
                case "ItemYellowKey": return Kind.ItemYellowKey.Some();
                case "EnemyAquamentus": return Kind.EnemyAquamentus.Some();
                case "EnemyZol": return Kind.EnemyZol.Some();
                case "EnemyGoriya": return Kind.EnemyGoriya.Some();
                case "EnemyDodongos": return Kind.EnemyDodongos.Some();
                case "EnemyStalfos": return Kind.EnemyStalfos.Some();
                case "EnemyRope": return Kind.EnemyRope.Some();
                case "EnemyKeese": return Kind.EnemyKeese.Some();
                case "EnemyWallmaster": return Kind.EnemyWallmaster.Some();
                case "ObstacleBlueDoor": return Kind.ObstacleBlueDoor.Some();
                case "ObstacleRedDoor": return Kind.ObstacleRedDoor.Some();
                case "ObstacleGreenDoor": return Kind.ObstacleGreenDoor.Some();
                case "ObstaclePurpleDoor": return Kind.ObstaclePurpleDoor.Some();
                case "ObstacleYellowDoor": return Kind.ObstacleYellowDoor.Some();
                default: return Option.None<Kind>();
            }
        }

        // Pre-condition: `spawnGroup` is the object group containing spawns
        public Spawns(TmxObjectGroup spawnGroup)
        {
            spawnPositions = new Dictionary<Kind, List<PhysicalVector2>>();

            foreach (TmxObject tmxObject in spawnGroup.Objects) {
                Option<Kind> kindOpt = GetKind(tmxObject.Type);
                kindOpt.Match(
                    some: kind => {
                        var spawnPosition = new PhysicalVector2(
                            (float)tmxObject.X,
                            (float)tmxObject.Y
                        );
                        if (spawnPositions.ContainsKey(kind)) {
                            spawnPositions[kind].Add(spawnPosition);
                        } else {
                            spawnPositions.Add(
                                kind,
                                new List<PhysicalVector2> { spawnPosition }
                            );
                        }
                    },
                    none: () => System.Console.WriteLine("Invalid spawn kind")
                );
            }
        }

        public PhysicalVector2 GetSpawnPosition(Kind kind)
        {
            Debug.Assert(spawnPositions.ContainsKey(kind));
            int i = Global.random.Next(spawnPositions[kind].Count);
            return spawnPositions[kind][i];
        }
    }
}
