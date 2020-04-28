using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Comps = NetGameShared.Ecs.Components;

using NetGameShared.Util.Physical;
using static NetGameShared.Util.Physical.Collision;
using static NetGameShared.Ecs.Components.Orientations.Cardinal;
using static NetGameShared.Maps.Util;

using Rectangle = NetGameShared.Util.Physical.Rectangle;
using PhysicalVector2 = Microsoft.Xna.Framework.Vector2;

using GameTile = NetGameShared.Maps.Tiles.Game;
using NetGameShared.Ecs.Components;

namespace NetGameShared.Ecs.Systems.Collision
{
    public static class SolidToMap
    {
        // Make this function inline so we don't have the overhead of passing in
        // a ton of parameters
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleCollision(
            Ecs.Registry registry,
            Queue<Entity> toRemove,
            Entity entity,
            Comps.Solid solidComp,
            Comps.Position positionComp,
            Comps.Shapes.Rectangle rectangleComp,
            PhysicalVector2 tilePosition,
            Rectangle tileRectangle
        ) {
            if (solidComp.ephemeral) {
                toRemove.Enqueue(entity);
            } else {
                PhysicalVector2 correction = GetMinCorrection(
                    rectangleComp.data, positionComp.data,
                    tileRectangle, tilePosition
                );

                positionComp.data += correction;
            }
        }

        public static void ProcessRemoves(
            Queue<Entity> toRemove,
            Ecs.Registry registry
        ) {
            while (toRemove.Count > 0) {
                Entity entity = toRemove.Dequeue();
                registry.Remove(entity);
            }
        }

        public static void Run(
            Ecs.Registry registry,
            IMap map
        ) {
            // To avoid performance costs from doing list intersections, only
            // query for entities with `Solid` components.
            //
            // However, we can assume that all entities with a `Solid` component
            // also have these components:
            // - `Position`
            // - `Shape.Rectangle`
            // - `Orientation.Cardinal`
            List<Entity> entities = registry.GetEntitiesList(typeof(Comps.Solid));

            // Queue entities to remove so we aren't removing entities while
            // iterating over them
            Queue<Entity> toRemove = new Queue<Entity>();

            foreach (Ecs.Entity entity in entities)
            {
                var entityRect = registry.GetComponentUnsafe<Comps.Shapes.Rectangle>(entity);
                var entityPos = registry.GetComponentUnsafe<Comps.Position>(entity);
                var entityPosRect = new PosRectangle(entityPos.data, entityRect.data);

                // Check every tile that the rectangle is touching.
                foreach (int index in OverlappingTiles(map, entityPosRect)) {
                    var gameTile = (GameTile)map.GameLayer.tiles[index];
                    switch (gameTile.kind)
                    {
                        case GameTile.Kind.Solid:
                            var entityOrientation = registry.GetComponentUnsafe<Comps.Orientations.Cardinal>(entity);
                            var entitySolid = registry.GetComponentUnsafe<Comps.Solid>(entity);

                            var tilePos = GetTilePosition(map, index);
                            var tileRect = new Rectangle(
                                map.TileWidth,
                                map.TileHeight
                            );
                            if (CheckCollision(
                                entityRect.data, entityPos.data, entityOrientation.data,
                                tileRect, tilePos, Util.Physical.Orientation.Cardinal.Right
                            ))
                            {
                                HandleCollision(
                                    registry, toRemove,
                                    entity, entitySolid, entityPos, entityRect,
                                    tilePos, tileRect
                                );
                            }

                            break;
                    }
                }
            }

            ProcessRemoves(toRemove, registry);
        }
    }
}
