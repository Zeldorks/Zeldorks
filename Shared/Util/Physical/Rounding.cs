using PhysicalVector2 = Microsoft.Xna.Framework.Vector2;

namespace NetGameShared.Util.Physical
{
    public static class Rounding
    {
        public const float epsilon = 0.001f;

        // Set float to zero if it becomes small enough
        public static float RoundIfZero(float f)
        {
            if (System.Math.Abs(f) < epsilon) {
                return 0.0f;
            } else {
                return f;
            }
        }

        // Set vector to zero if it becomes small enough
        public static PhysicalVector2 RoundIfZero(PhysicalVector2 vector2)
        {
            return new PhysicalVector2(
                RoundIfZero(vector2.X),
                RoundIfZero(vector2.Y)
            );
        }
    }
}
