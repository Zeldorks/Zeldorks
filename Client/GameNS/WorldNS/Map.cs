using System.Collections.Generic;
using NetGameShared;
using NetGameShared.Util;
using NetGameShared.Maps;
using NetGameClient.GameNS.WorldNS.MapsExt;
using Maps = NetGameShared.Maps;
using static NetGameShared.Maps.Tiles.Game;
using TiledSharp;

namespace NetGameClient.GameNS.WorldNS
{
    public class Map : IMap
    {
        public List<Layer> layers;

        // Index of game layer in `layers`
        public int GameLayerIndex { get; private set; }
        public Layer GameLayer
        {
            get { return layers[GameLayerIndex]; }
        }

        public List<Tileset> tilesets;

        // Index of game tileset in `tilesets
        public int GameTilesetIndex { get; private set; }
        public Tileset GameTileset
        {
            get { return tilesets[GameTilesetIndex]; }
        }
        
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

            int nLayers = tmxMap.TileLayers.Count;
            layers = new List<Layer>(nLayers);
            for (var i = 0; i < nLayers; i++) {
                var tmxLayer = tmxMap.TileLayers[i];
                var layer = new Layer();

                if (IsGameLayer(tmxLayer)) {
                    layer.Init<Maps.Tiles.Game>(tmxLayer, tmxMap);
                    GameLayerIndex = i;
                } else {
                    layer.Init<MapsExt.Tiles.Visible>(tmxLayer, tmxMap);
                }

                layers.Add(layer);
            }

            int nTilesets = tmxMap.Tilesets.Count;
            tilesets = new List<Tileset>(nTilesets);
            for (var i = 0; i < nTilesets; i++) {
                TmxTileset tmxTileset = tmxMap.Tilesets[i];

                if (Tileset.IsGameTileset(tmxTileset)) {
                    GameTilesetIndex = i;
                }

                var tileset = new Tileset(tmxTileset, tmxMap);
                tilesets.Add(tileset);
            }
        }

        public void DebugPrint(Layer layer)
        {
            for (var i = 0; i < layer.tiles.Count; i++) {
                Tile absTile = layer.tiles[i];
                if (absTile is MapsExt.Tiles.Visible) {
                    var tile = (MapsExt.Tiles.Visible)absTile;
                    System.Console.Write("{0} ", tile.gid);
                } else {
                    var tile = (Maps.Tiles.Game)absTile;
                    System.Console.Write("{0} ", tile.kind);
                }

                if (i % Width == 0) {
                    System.Console.WriteLine("");
                }
            }
        }

        public void DebugPrint()
        {
            System.Console.WriteLine("Map Layers:");
            for (var i = 0; i < layers.Count; i++) {
                var layer = layers[i];
                System.Console.WriteLine("    Layer {0}:", i);
                DebugPrint(layer);
            }
        }
    }
}
