using System;
using PhysicalVector2 = Microsoft.Xna.Framework.Vector2;

namespace NetGameShared.Util.Physical
{
    // If C# had public typedefs, I would typedef this:
    // `public typedef Vector2 = Microsoft.Xna.Framework.Vector2`

    public struct Rectangle : IEquatable<Rectangle>
    {
        public float Width { get; set; }
        public float Height { get; set; }

        public Rectangle(float width, float height)
        {
            Width = width;
            Height = height;
        }

        public Rectangle(PhysicalVector2 vector)
        {
            Width = vector.X;
            Height = vector.Y;
        }

        public PhysicalVector2 ToVector2()
        {
            return new PhysicalVector2(Width, Height);
        }

        public override int GetHashCode()
        {
            // Reference: https://stackoverflow.com/a/5221407
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + Width.GetHashCode();
                hash = hash * 23 + Height.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Rectangle)
            {
                return Equals((Rectangle)obj);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(Rectangle rectangle)
        {
            return Width == rectangle.Width && Height == rectangle.Height;
        }

        public static bool operator ==(Rectangle lhs, Rectangle rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Rectangle lhs, Rectangle rhs)
        {
            return !lhs.Equals(rhs);
        }

        // ---

        public static Rectangle operator *(Rectangle rectangle, float scalar)
        {
            return new Rectangle(
                rectangle.Width * scalar,
                rectangle.Height * scalar
            );
        }

        public static Rectangle operator /(Rectangle rectangle, float scalar)
        {
            return new Rectangle(
                rectangle.Width / scalar,
                rectangle.Height / scalar
            );
        }
    }
}
