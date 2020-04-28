using TiledSharp;

namespace NetGameShared.Maps
{
    public abstract class Tile
    {
        public abstract void Init(
            TmxLayerTile tmxLayerTile,
            TmxMap tmxMap
        );
    }
}
