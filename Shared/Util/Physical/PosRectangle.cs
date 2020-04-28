using PhysicalVector2 = Microsoft.Xna.Framework.Vector2;

namespace NetGameShared.Util.Physical
{
    // Rectangle with position
    public class PosRectangle
    {
        public PhysicalVector2 Center { get; set; }
        public Rectangle Rectangle { get; set; }

        public PosRectangle(
            PhysicalVector2 position,
            Rectangle rectangle
        ) {
            Center = position;
            Rectangle = rectangle;
        }

        public float Left {
            get => Center.X - (Rectangle.Width * 0.5f);
        }

        public float Right {
            get => Center.X + (Rectangle.Width * 0.5f);
        }

        public float Top {
            get => Center.Y - (Rectangle.Height * 0.5f);
        }

        public float Bottom {
            get => Center.Y + (Rectangle.Height * 0.5f);
        }

        public float GetBound(Orientation.Cardinal orientation)
        {
            switch (orientation) {
                case Orientation.Cardinal.Left: return Left;
                case Orientation.Cardinal.Right: return Right;
                case Orientation.Cardinal.Up: return Top;
                case Orientation.Cardinal.Down: return Bottom;
                default: throw new System.ArgumentException("Invalid orientation");
            }
        }

        public PhysicalVector2 GetCenteredBound(Orientation.Cardinal orientation)
        {
            switch (orientation) {
                case Orientation.Cardinal.Left:
                    return new PhysicalVector2(Left, Center.Y);
                case Orientation.Cardinal.Right:
                    return new PhysicalVector2(Right, Center.Y);
                case Orientation.Cardinal.Up:
                    return new PhysicalVector2(Center.X, Top);
                case Orientation.Cardinal.Down:
                    return new PhysicalVector2(Center.X, Bottom);
                default:
                    throw new System.ArgumentException("Invalid orientation");
            }
        }
    }
}
