using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using NetGameClient.Util;
using DrawingVector = Microsoft.Xna.Framework.Point;
using DrawingPosRectangle = Microsoft.Xna.Framework.Rectangle;

namespace NetGameClient.GameNS.WorldNS.EcsExt.Components.Visibles
{
    public class Sprite : Visible
    {
        public Texture2D texture;
        public DrawingPosRectangle texturePosRectangle;
        public DrawingVector destOffset;
        public Drawing.Rectangle destRectangle;
        public Color tint = Color.White;
        public float rotation = 0.0f;

        // Horizontal and vertical flip
        public SpriteEffects effects = SpriteEffects.None;
    }
}
