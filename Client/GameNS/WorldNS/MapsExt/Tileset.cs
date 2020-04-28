using TiledSharp;

namespace NetGameClient.GameNS.WorldNS.MapsExt
{
    public class Tileset
    {
        // Based on `TiledSharp.TmxTileset`
        public int firstGid;
        public string name;

        // The dimensions of the tileset image (Units: tiles)
        public int width;
        public int height;

        public Tileset(TmxTileset tmxTileset, TmxMap map)
        {
            firstGid = tmxTileset.FirstGid;
            name = tmxTileset.Name;

            // TODO: Handle when this it's null
            int imageWidth = tmxTileset.Image.Width.GetValueOrDefault();
            int imageHeight = tmxTileset.Image.Height.GetValueOrDefault();

            width = imageWidth / map.TileWidth;
            height = imageHeight / map.TileHeight;
        }

        public static bool IsGameTileset(TmxTileset tmxTileset)
        {
            return tmxTileset.Name == "Game";
        }
    }
}
