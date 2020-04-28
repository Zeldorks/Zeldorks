using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using GameTile = NetGameShared.Maps.Tiles.Game;
using static NetGameShared.Maps.Util;
using static NetGameClient.GameNS.WorldNS.MapsExt.Util;

using Ecs = NetGameShared.Ecs;

using Drawing = NetGameClient.Util.Drawing;
using DrawingPosRectangle = Microsoft.Xna.Framework.Rectangle;
using PhysicalVector2 = Microsoft.Xna.Framework.Vector2;

namespace NetGameClient.GameNS.WorldNS.MapsExt
{
    public static class Renderer
    {
        // Get the `DrawingPosRectangle` for a tile in its tileset
        public static DrawingPosRectangle GetTexturePosRectangle(
            int tileGid,
            Tileset tileset,
            Map map
        ) {
            int tileFrame = GetTileFrame(tileGid, tileset);
            (int tileRow, int tileColumn) = GetColumnAndRow(tileFrame, tileset);
            return new DrawingPosRectangle(
                map.TileWidth * tileColumn,
                map.TileHeight * tileRow,
                map.TileWidth,
                map.TileHeight
            );
        }

        public static DrawingPosRectangle GetTexturePosRectangleWithTileFrame(
            int tileFrame,
            Tileset tileset,
            Map map
        ) {
            (int tileRow, int tileColumn) = GetColumnAndRow(tileFrame, tileset);
            return new DrawingPosRectangle(
                map.TileWidth * tileColumn,
                map.TileHeight * tileRow,
                map.TileWidth,
                map.TileHeight
            );
        }

        public static Drawing.Rectangle GetDestRectangle(
            Map map
        ) {
            return new Drawing.Rectangle() {
                Width = map.TileWidth,
                Height = map.TileHeight
            };
        }

        // Get information on flipping the tile
        public static SpriteEffects GetSpriteEffects(
            Tiles.Visible tile
        ) {
            if (!tile.horizontalFlip &&
                !tile.verticalFlip &&
                !tile.diagonalFlip
            ) {
                return SpriteEffects.None;
            }

            if (tile.horizontalFlip &&
                !tile.verticalFlip &&
                !tile.diagonalFlip
            ) {
                return SpriteEffects.FlipHorizontally;
            }

            if (!tile.horizontalFlip &&
                tile.verticalFlip &&
                !tile.diagonalFlip
            ) {
                return SpriteEffects.FlipVertically;
            }

            if (tile.horizontalFlip &&
                tile.verticalFlip &&
                !tile.diagonalFlip
            ) {
                return SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
            }

            if (tile.horizontalFlip &&
                !tile.verticalFlip &&
                tile.diagonalFlip
            ) {
                // TODO: Rotate 90 deg clockwise
                Console.WriteLine("[WARN] Unsupported tile rotation (90 deg)");
                return SpriteEffects.None;
            }

            if (!tile.horizontalFlip &&
                tile.verticalFlip &&
                tile.diagonalFlip
            ) {
                // TODO: Rotate 270 deg clockwise
                Console.WriteLine("[WARN] Unsupported tile rotation (270 deg)");
                return SpriteEffects.None;
            }

            Console.WriteLine("[WARN] Unknown tile transform");
            Console.WriteLine("[WARN] HFlip: {0}", tile.horizontalFlip);
            Console.WriteLine("[WARN] VFlip: {0}", tile.verticalFlip);
            Console.WriteLine("[WARN] DFlip: {0}", tile.diagonalFlip);
            return SpriteEffects.None;
        }

        private static Texture2D GetTexture(
            ContentManager contentManager,
            Tileset tileset
        ) {
            return contentManager.Load<Texture2D>(
                "MapImages/" + tileset.name.ToString()
            );
        }

        private static void Render(
            Tiles.Visible tile,
            int tileIndex, // Tile index in its layer
            Map map,
            ContentManager contentManager,
            SpriteBatch spriteBatch,
            Camera camera
        ) {
            if (tile.gid == 0) {
                // Skip empty tiles
                return;
            }

            PhysicalVector2 tilePos = GetTilePosition(map, tileIndex);
            var position = new Ecs.Components.Position() {
                data = tilePos
            };

            Tileset tileset = GetTilesetOfTile(map, tile.gid);
            Texture2D tilesetTexture = GetTexture(contentManager, tileset);
            var texturePosRectangle = GetTexturePosRectangle(
                tile.gid,
                tileset,
                map
            );
            var destRectangle = GetDestRectangle(map);
            SpriteEffects effects = GetSpriteEffects(tile);

            var sprite = new EcsExt.Components.Visibles.Sprite() {
                texture = tilesetTexture,
                texturePosRectangle = texturePosRectangle,
                destRectangle = destRectangle,
                tint = Color.White,
                effects = effects
            };
            int starttime = DateTime.Now.Millisecond;
            EcsExt.Systems.SpriteRender.Render(
                sprite,
                position,
                spriteBatch,
                camera
            );
        }

        private static void Render(
            GameTile tile,
            int tileIndex, // Tile index in its layer
            Map map,
            ContentManager contentManager,
            SpriteBatch spriteBatch,
            Camera camera
        ) {
            if (tile.kind == GameTile.Kind.Empty) {
                // Skip empty tiles
                return;
            }

            PhysicalVector2 tilePos = GetTilePosition(map, tileIndex);
            var position = new Ecs.Components.Position() {
                data = tilePos
            };

            Tileset tileset = map.GameTileset;
            Texture2D tilesetTexture = GetTexture(contentManager, tileset);
            int tileFrame = tile.GetTileFrame;
            var texturePosRectangle = GetTexturePosRectangleWithTileFrame(
                tileFrame,
                tileset,
                map
            );
            var destRectangle = GetDestRectangle(map);

            var sprite = new EcsExt.Components.Visibles.Sprite() {
                texture = tilesetTexture,
                texturePosRectangle = texturePosRectangle,
                destRectangle = destRectangle,
                tint = Color.White
            };

            EcsExt.Systems.SpriteRender.Render(
                sprite,
                position,
                spriteBatch,
                camera
            );
        }

        public static bool renderGameLayer = false;

        public static void Render(
            Map map,
            ContentManager contentManager,
            SpriteBatch spriteBatch,
            Camera camera
        ) {
            for (var i = 0; i < map.layers.Count; i++) {
                var layer = map.layers[i];

                if (i == map.GameLayerIndex) {
                    if (renderGameLayer) {
                        // Render game layer
                        foreach (int j in OverlappingTiles(map, camera.Range)) {
                            var tile = (GameTile)layer.tiles[j];
                            Render(tile, j, map, contentManager, spriteBatch, camera);
                        }
                    }
                } else {
                    // Render visible layer
                    foreach (int j in OverlappingTiles(map, camera.Range)) {
                        var tile = (Tiles.Visible)layer.tiles[j];
                        Render(tile, j, map, contentManager, spriteBatch, camera);
                    }
                }
            }
        }
    }
}
