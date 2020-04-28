using System.Collections.Generic;
using NetGameShared.Net.Protocol.GameInputs;
using GameInputs = NetGameShared.Net.Protocol.GameInputs;
using PositionComp = NetGameShared.Ecs.Components.Position;
using VelocityComp = NetGameShared.Ecs.Components.Velocity;
using static NetGameShared.Util.Physical.Rounding;

namespace NetGameShared.Ecs.Systems
{
    public static class Movement
    {
        // Pre-condition: `movement != GameInputs.Movement.None`
        public static void Process(
            GameInputs.Movement movement,
            Entity entity,
            Registry registry
        ) {
            registry.AssignComponent(
                entity,
                new VelocityComp {
                    data = movement.ToVector2() * 15
                }
            );
        }

        public static void Run(Registry registry)
        {
            HashSet<Entity> entities = registry.GetEntities(
                typeof(PositionComp),
                typeof(VelocityComp)
            );

            foreach (Entity entity in entities) {
                VelocityComp velocityComp = registry
                    .GetComponentUnsafe<VelocityComp>(entity);

                PositionComp positionComp = registry
                    .GetComponentUnsafe<PositionComp>(entity);

                // Decay velocity to simulate friction
                velocityComp.data *= velocityComp.decay;
                velocityComp.data = RoundIfZero(velocityComp.data);

                positionComp.data += velocityComp.data;
            }
        }
    }
}
