using PhysicalVector2 = Microsoft.Xna.Framework.Vector2;

namespace NetGameShared.Util.Physical.Orientation
{
    public enum Cardinal
    {
        Up, Left, Down, Right
    }

    public static class CardinalExt
    {
        public static PhysicalVector2 GetPhysicalVector2(
            this Orientation.Cardinal cardinal
        ) {
            switch (cardinal) {
                case Physical.Orientation.Cardinal.Up:
                    return new PhysicalVector2(0.0f, -1.0f);
                case Physical.Orientation.Cardinal.Down:
                    return new PhysicalVector2(0.0f, 1.0f);
                case Physical.Orientation.Cardinal.Left:
                    return new PhysicalVector2(-1.0f, 0.0f);
                case Physical.Orientation.Cardinal.Right:
                    return new PhysicalVector2(1.0f, 0.0f);
                default:
                    throw new System.ArgumentException(
                        "Invalid orientation direction"
                    );
            }
        }
    }
}
