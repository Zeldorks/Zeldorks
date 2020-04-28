using System;
using PhysicalVector2 = Microsoft.Xna.Framework.Vector2;
using Physical = NetGameShared.Util.Physical;

namespace NetGameShared.Maps
{
    public static class Util
    {
        // Get the physical position of tile given it's index in its layer
        public static PhysicalVector2 GetTilePosition(
            IMap map,
            int tileIndex
        ) {
            return new PhysicalVector2() {
                X = ((tileIndex % map.Width) * map.TileWidth) +
                    (map.TileWidth / 2),
                Y = ((float)Math.Floor(tileIndex / (double)map.Width) * map.TileHeight) +
                    (map.TileHeight / 2)
            };
        }

        public static System.Collections.IEnumerable OverlappingTiles(
            IMap map,
            Physical.PosRectangle posRectangle
        ) {
            var startX = (int)(posRectangle.Left / map.TileWidth);
            var startY = (int)(posRectangle.Top / map.TileHeight);
            var endX = (int)(posRectangle.Right / map.TileWidth) + 1;
            var endY = (int)(posRectangle.Bottom / map.TileHeight) + 1;

            startX = Math.Clamp(startX, 0, map.Width - 1);
            endX = Math.Clamp(endX, 0, map.Width - 1);
            startY = Math.Clamp(startY, 0, map.Height - 1);
            endY = Math.Clamp(endY, 0, map.Height - 1);

            for (int x = startX; x <= endX; x++) {
                for (int y = startY; y <= endY; y++) {
                    int index = x + y * map.Width;
                    yield return index;
                }
            }
        }
    }
}
