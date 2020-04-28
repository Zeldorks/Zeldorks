using NetGameShared.Ecs;
using Ecs = NetGameShared.Ecs;
using Comps = NetGameShared.Ecs.Components;

using Physical = NetGameShared.Util.Physical;
using PhysicalVector2 = Microsoft.Xna.Framework.Vector2;

namespace NetGameServer.GameNS.WorldNS.EcsExt.Factories
{
    public static class Item
    {
        public static Entity Create(
            Comps.Item.Kind kind,
            PhysicalVector2 position,
            Ecs.Registry registry
        ) {
            var result = registry.CreateEntity();
            registry.AssignComponent(
                result,
                new Comps.Item { kind = kind }
            );

            registry.AssignComponent(
                result,
                new Comps.Position { data = position }
            );

            // TODO: Figure out what shape to assign
            registry.AssignComponent(
                result,
                new Comps.Shapes.Rectangle {
                    data = new Physical.Rectangle(64.0f, 64.0f)
                }
            );

            return result;
        }

    }
}
