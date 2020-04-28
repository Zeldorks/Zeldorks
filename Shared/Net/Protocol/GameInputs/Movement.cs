using Microsoft.Xna.Framework;
using Physical = NetGameShared.Util.Physical;

namespace NetGameShared.Net.Protocol.GameInputs
{
    public enum Movement
    {
        None, Left, Right, Up, Down
    }

    public static class MovementExtensions
    {
        public static Vector2 ToVector2(this Movement movement)
        {
            switch (movement)
            {
                case GameInputs.Movement.Left:
                    return new Vector2(-1.0f, 0.0f);
                case GameInputs.Movement.Right:
                    return new Vector2(1.0f, 0.0f);
                case GameInputs.Movement.Up:
                    return new Vector2(0.0f, -1.0f);
                case GameInputs.Movement.Down:
                    return new Vector2(0.0f, 1.0f);
                default:
                    throw new System.ArgumentException("Invalid movement");
            }
        }

        public static Physical.Orientation.Cardinal ToCardinalDirection(this Movement movement)
        {
            switch (movement)
            {
                case Movement.Left:
                    return Physical.Orientation.Cardinal.Left;
                case Movement.Right:
                    return Physical.Orientation.Cardinal.Right;
                case Movement.Up:
                    return Physical.Orientation.Cardinal.Up;
                case Movement.Down:
                    return Physical.Orientation.Cardinal.Down;
                default:
                    throw new System.ArgumentException("Invalid movement");
            }
        }
    }
}
