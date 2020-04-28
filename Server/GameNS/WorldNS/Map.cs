using TiledSharp;
using NetGameShared;
using NetGameShared.Util;
using NetGameShared.Maps;
using Maps = NetGameShared.Maps;
using static NetGameShared.Maps.Tiles.Game;
using NetGameServer.GameNS.WorldNS.MapsExt;

namespace NetGameServer.GameNS.WorldNS
{
    public class Map : IMap
    {
        // Index of game layer in `layers`
        public Layer GameLayer { get; private set; }
        public Spawns spawns;
        public Teleports teleports;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }

        public Map(string mapName)
        {
            var tmxMap =  new TmxMap(
                Paths.AssemblyDirectory + "/Content/Maps/" + mapName + ".tmx"
            );

            Width = tmxMap.Width;
            Height = tmxMap.Height;
            TileWidth = tmxMap.TileWidth;
            TileHeight = tmxMap.TileHeight;

            for (var i = 0; i < tmxMap.TileLayers.Count; i++) {
                var tmxLayer = tmxMap.TileLayers[i];
                var layer = new Layer();

                if (IsGameLayer(tmxLayer)) {
                    layer.Init<Maps.Tiles.Game>(tmxLayer, tmxMap);
                    GameLayer = layer;
                }
            }

            for (var i = 0; i < tmxMap.ObjectGroups.Count; i++) {
                var tmxObjectGroup = tmxMap.ObjectGroups[i];
                switch (tmxObjectGroup.Name) {
                    case "Spawns":
                        spawns = new Spawns(tmxObjectGroup);
                        break;
                    case "Teleports":
                        teleports = new Teleports(tmxObjectGroup);
                        break;
                    default:
                        System.Console.WriteLine("[WARN] Unknown object group");
                        break;
                }
            }
        }

        public void DebugPrint()
        {
            var layer = GameLayer;
            System.Console.WriteLine("Game layer:");
            for (var i = 0; i < layer.tiles.Count; i++) {
                Tile absTile = layer.tiles[i];
                var tile = (Maps.Tiles.Game)absTile;
                System.Console.Write("{0} ", tile.kind);

                if (i % Width == 0) {
                    System.Console.WriteLine("");
                }
            }
        }
    }
}
