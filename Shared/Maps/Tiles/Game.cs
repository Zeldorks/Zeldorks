using TiledSharp;
using static NetGameShared.Maps.TmxUtil;

namespace NetGameShared.Maps.Tiles
{
    public class Game : Tile
    {
        public enum Kind
        {
            Empty,
            Solid
        }

        public Kind kind;

        public static Kind GetKind(int tileFrame)
        {
            switch (tileFrame) {
                case 0:
                   return Kind.Empty; 
               case 1:
                    return Kind.Solid;
                default:
                    System.Console.WriteLine(
                        "[WARN] Unknown game tile frame: {0}",
                        tileFrame
                    );
                    return Kind.Empty;
            }
        }

        public int GetTileFrame
        {
            get {
                switch (kind) {
                    case Kind.Empty:
                        return 0;
                    case Kind.Solid:
                        return 1;
                    default:
                        throw new System.ArgumentException(
                            "Invalid game tile kind"
                        );
                }
            }
        }

        public override void Init(
            TmxLayerTile tmxLayerTile,
            TmxMap tmxMap
        ) {
            int tileGid = tmxLayerTile.Gid;
            int tileFrame = TmxUtil.GetTileFrame(tileGid, tmxMap);
            kind = GetKind(tileFrame);
        }

        public static bool IsGameLayer(TmxLayer tmxLayer)
        {
            return tmxLayer.Name == "Game";
        }
    }
}
