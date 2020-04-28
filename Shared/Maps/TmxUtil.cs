using TiledSharp;

namespace NetGameShared.Maps
{
    public static class TmxUtil
    {
        // Get the tileset containing a tile specified by its GID
        public static TmxTileset GetTilesetOfTile(TmxMap map, int tileGid)
        {
            TmxTileset result = map.Tilesets[0];
            foreach (TmxTileset tileset in map.Tilesets)
            {
                if (tileGid < tileset.FirstGid) {
                    return result;
                } else {
                    result = tileset;
                }
            }
            return result;
        }

        // Get the frame of a tile relative to its tileset
        public static int GetTileFrame(int tileGid, TmxTileset tileset)
        {
            return tileGid - tileset.FirstGid;
        }

        // Get the frame of a tile relative to its tileset
        public static int GetTileFrame(int tileGid, TmxMap tmxMap)
        {
            if (tileGid == 0) return 0;
            TmxTileset tileset = GetTilesetOfTile(tmxMap, tileGid);
            return GetTileFrame(tileGid, tileset);
        }
    }
}
