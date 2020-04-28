using System.Diagnostics;
using System.Collections.Generic;
using TiledSharp;

namespace NetGameShared.Maps
{
    public class Layer
    {
        public List<Tile> tiles;

        public void Init<T>(
            TmxLayer tmxLayer,
            TmxMap tmxMap
        ) where T : Tile, new()
        {
            int nTiles = tmxLayer.Tiles.Count;
            tiles = new List<Tile>(nTiles);
            for (var i = 0; i < nTiles; i++) {
                TmxLayerTile tmxLayerTile = tmxLayer.Tiles[i];
                T tile = new T();
                tile.Init(tmxLayerTile, tmxMap);
                tiles.Add(tile);
            }
        }
    }
}
