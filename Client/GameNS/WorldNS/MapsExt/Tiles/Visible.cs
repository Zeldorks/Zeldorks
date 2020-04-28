using TiledSharp;
using NetGameShared.Maps;

namespace NetGameClient.GameNS.WorldNS.MapsExt.Tiles
{
    public class Visible : Tile
    {
        // Based on `TiledSharp.TmxLayerTile`
        public int gid;
        public bool horizontalFlip;
        public bool verticalFlip;
        public bool diagonalFlip;

        public override void Init(
            TmxLayerTile tmxLayerTile,
            TmxMap _
        ) {
            gid = tmxLayerTile.Gid;
            horizontalFlip = tmxLayerTile.HorizontalFlip;
            verticalFlip = tmxLayerTile.VerticalFlip;
            diagonalFlip = tmxLayerTile.DiagonalFlip;
        }
    }
}
