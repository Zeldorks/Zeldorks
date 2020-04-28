using NetGameShared.Ecs;
using Ecs = NetGameShared.Ecs;
using Comps = NetGameShared.Ecs.Components;

using Physical = NetGameShared.Util.Physical;
using PhysicalVector2 = Microsoft.Xna.Framework.Vector2;

namespace NetGameServer.GameNS.WorldNS.EcsExt.Factories
{
    public static class Door
    {
        public static Entity Create(
            Comps.Door.Kind kind,
            PhysicalVector2 position,
            Ecs.Registry registry
        ) {
            var result = registry.CreateEntity();
            registry.AssignComponent(
                result,
                new Comps.Door { kind = kind }
            );

            registry.AssignComponent(
                result,
                new Comps.Solid { unmovable = true }
            );

            registry.AssignComponent(
                result,
                new Comps.Position { data = position }
            );

            registry.AssignComponent(
                result,
                new Comps.Shapes.Rectangle {
                    data = new Physical.Rectangle(32.0f * 6, 32.0f * 2)
                }
            );

            return result;
        }
    }
}
