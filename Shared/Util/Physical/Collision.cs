using PhysicalVector2 = Microsoft.Xna.Framework.Vector2;

namespace NetGameShared.Util.Physical
{
    public static class Collision
    {
        public static bool IsCollisionPossible(
            PhysicalVector2 position1,
            PhysicalVector2 position2
        ) {
            float distance = PhysicalVector2.Distance(position1, position2);
            return distance < 64.0f * 6;
        }

        public static bool CheckCollision(
            Rectangle rectangle1,
            PhysicalVector2 position1,
            Orientation.Cardinal cardinal1,
            Rectangle rectangle2,
            PhysicalVector2 position2,
            Orientation.Cardinal cardinal2
         ) {

            var copyPos1 = position1;
            var copyPos2 = position2;

            if (cardinal1 == Orientation.Cardinal.Up || cardinal1 == Orientation.Cardinal.Down)
            {
                var temp = copyPos1.X;
                copyPos1.X = copyPos1.Y;
                copyPos1.Y = temp;
            }

            if (cardinal2 == Orientation.Cardinal.Up || cardinal2 == Orientation.Cardinal.Down)
            {
                var temp = copyPos2.X;
                copyPos2.X = copyPos2.Y;
                copyPos2.Y = temp;
            }  
            var posRect1 = new PosRectangle(position1, rectangle1);
            var posRect2 = new PosRectangle(position2, rectangle2);

            // Check for collision.
            return
                posRect1.Left < posRect2.Right &&
                posRect1.Right > posRect2.Left &&
                posRect1.Top < posRect2.Bottom &&
                posRect1.Bottom > posRect2.Top;
        }

        // Find the smallest correction we can apply to the first rectangle that
        // makes it so the rectangles are no longer colliding.
        public static PhysicalVector2 GetMinCorrection(
            Rectangle rectangle1,
            PhysicalVector2 position1,
            Rectangle rectangle2,
            PhysicalVector2 position2
        ) {
            var posRect1 = new PosRectangle(position1, rectangle1);
            var posRect2 = new PosRectangle(position2, rectangle2);

            var xCorrection = new PhysicalVector2(0, 0);
            var yCorrection = new PhysicalVector2(0, 0);

            // Check how far we have to push the rectangle out on the x direction.
            if (position1.X <= position2.X) {
                xCorrection.X = posRect2.Left - posRect1.Right;
            } else {
                xCorrection.X = posRect2.Right - posRect1.Left;
            }

            // Check how far we have to push the rectangle out on the y direction.
            if (position1.Y <= position2.Y) {
                yCorrection.Y = posRect2.Top - posRect1.Bottom;
            } else {
                yCorrection.Y = posRect2.Bottom - posRect1.Top;
            }

            // See which correction pushes the rectangle out the least and use that as our correction.
            if (xCorrection.Length() < yCorrection.Length()) {
                return xCorrection;
            } else {
                return yCorrection;
            }
        }
    }
}
