namespace NetGameClient.GameNS.WorldNS.MapsExt
{
    public static class Util
    {
        // Get the tileset containing a tile specified by its GID
        public static Tileset GetTilesetOfTile(
            Map map,
            int tileGid
        ) {
            Tileset result = map.tilesets[0];
            foreach (Tileset tileset in map.tilesets) {
                if (tileGid < tileset.firstGid) {
                    return result;
                } else {
                    result = tileset;
                }
            }

            return result;
        }

        // Get the frame of a tile relative to its tileset
        public static int GetTileFrame(int tileGid, Tileset tileset)
        {
            return tileGid - tileset.firstGid;
        }

        // Get the row and column of a tile in its tileset
        public static (int row, int column) GetColumnAndRow(
            int tileFrame,
            Tileset tileset
        ) {
            int row = tileFrame / tileset.width;
            int column = tileFrame % tileset.width;
            return (row, column);
        }
    }
}
