using NetGameShared.Net.Protocol.GameInputs;
using GameInputs = NetGameShared.Net.Protocol.GameInputs;

using Comps = NetGameShared.Ecs.Components;
using Physical = NetGameShared.Util.Physical;

namespace NetGameShared.Ecs.Systems
{
    public static class CharacterOrientation
    {
        // Pre-condition: `movement != GameInputs.Movement.None`
        public static void Process(
            GameInputs.Movement movement,
            Entity entity,
            Registry registry
        ) {
            var character = registry
                .GetComponentUnsafe<Comps.Character>(entity);

            var orientation = registry
                .GetComponentUnsafe<Comps.Orientations.Cardinal>(entity);

            Physical.Orientation.Cardinal movementDirection =
                movement.ToCardinalDirection();

            if (character.kind == Comps.Character.Kind.Aquamentus) {
                // Aquamentus can only face left and right
                switch (movementDirection) {
                    case Physical.Orientation.Cardinal.Left:
                    case Physical.Orientation.Cardinal.Right:
                        orientation.data = movementDirection;
                        break;
                }
            } else {
                orientation.data = movementDirection;
            }
        }
    }
}
