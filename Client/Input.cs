using GameInputs = NetGameShared.Net.Protocol.GameInputs;

namespace NetGameClient
{
    public enum Input
    {
        Connect, Disconnect,
        MoveLeft, MoveRight, MoveUp, MoveDown,
        UseSlotA, UseSlotB,
        NextItemSlotA, PrevItemSlotA,
        NextItemSlotB, PrevItemSlotB,

        // Print useful debug information to console
        DebugPrint,

        // Render useful debugging information to the screen
        DebugRender,
        DebugUnRender,

        // Don't send any packets to the server to simulate losing the
        // connection
        DebugDisconnect,
        DebugUnDisconnect,

        Quit
    }

    public static class InputExt
    {
        public static GameInputs.Movement GetGameInputMovement(this Input input)
        {
            switch (input) {
                case Input.MoveUp:
                    return GameInputs.Movement.Up;
                case Input.MoveLeft:
                    return GameInputs.Movement.Left;
                case Input.MoveDown:
                    return GameInputs.Movement.Down;
                case Input.MoveRight:
                    return GameInputs.Movement.Right;
                default:
                    throw new System.ArgumentException("Input is not a movement");
            }
        }
    }
}
