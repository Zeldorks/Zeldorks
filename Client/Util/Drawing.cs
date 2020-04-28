using DrawingVector = Microsoft.Xna.Framework.Point;
using DrawingPosRectangle = Microsoft.Xna.Framework.Rectangle;

namespace NetGameClient.Util
{
    public static class Drawing
    {
        // If C# had public typedefs, I would typedef these:
        //
        // Drawing vectors are always 2D, so there's no need to be `Vector2`
        // `public typedef Vector = Microsoft.Xna.Framework.Point`
        //
        // A rectangle with position
        // `public typedef PosRectangle = Microsoft.Xna.Framework.Rectangle`

        public struct Rectangle
        {
            public int Width { get; set; }
            public int Height { get; set; }

            public Rectangle(int width, int height)
            {
                Width = width;
                Height = height;
            }

            public DrawingVector ToVector()
            {
                return new DrawingVector(Width, Height);
            }

            public static Rectangle operator*(Rectangle rectangle, float scalar)
            {
                return new Rectangle(
                    (int)(rectangle.Width * scalar),
                    (int)(rectangle.Height * scalar)
                );
            }
        }

        public static DrawingPosRectangle GetCenteredDrawingPosRectangle(
            DrawingVector position,
            Rectangle rectangle
        ) {
            return new DrawingPosRectangle(
                position.X - (int)(rectangle.Width / 2),
                position.Y - (int)(rectangle.Height / 2),
                rectangle.Width,
                rectangle.Height
            );
        }
    }
}
