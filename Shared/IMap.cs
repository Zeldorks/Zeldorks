namespace NetGameShared
{
    public interface IMap
    {
        // Every map must have a game layer
        Maps.Layer GameLayer { get; }
        
        int Width { get; }
        int Height { get; }
        int TileWidth { get; }
        int TileHeight { get; }
    }
}
