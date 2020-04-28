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

            Physical.Rectangle rectangle;
            switch (kind)
            {
                case Comps.Item.Kind.Bomb:
                    rectangle = new Physical.Rectangle(
                        8.0f * 4,
                        14.0f * 4
                    );
                    break;
                case Comps.Item.Kind.Boomerang:
                    rectangle = new Physical.Rectangle(
                        5.0f * 4,
                        8.0f * 4
                    );
                    break;
                case Comps.Item.Kind.Bow:
                    rectangle = new Physical.Rectangle(
                        8.0f * 4,
                        16.0f * 4
                    );
                    break;
                case Comps.Item.Kind.Candle:
                    rectangle = new Physical.Rectangle(
                        8.0f * 4,
                        16.0f * 4
                    );
                    break;
                case Comps.Item.Kind.Clock:
                    rectangle = new Physical.Rectangle(
                        11.0f * 4,
                        16.0f * 4
                    );
                    break;
                case Comps.Item.Kind.Compass:
                    rectangle = new Physical.Rectangle(
                        11.0f * 4,
                        12.0f * 4
                    );
                    break;
                case Comps.Item.Kind.Heart:
                    rectangle = new Physical.Rectangle(
                        7.0f * 4,
                        8.0f * 4
                    );
                    break;
                case Comps.Item.Kind.Map:
                    rectangle = new Physical.Rectangle(
                        8.0f * 4,
                        16.0f * 4
                    );
                    break;
                case Comps.Item.Kind.Rupee:
                    rectangle = new Physical.Rectangle(
                        8.0f * 4,
                        16.0f * 4
                    );
                    break;
                case Comps.Item.Kind.Sword:
                    rectangle = new Physical.Rectangle(
                        7.0f * 4,
                        16.0f * 4
                    );
                    break;
                case Comps.Item.Kind.TriforceShard:
                    rectangle = new Physical.Rectangle(
                        10.0f * 4,
                        10.0f * 4
                    );
                    break;
                default:
                    rectangle = new Physical.Rectangle(
                        16.0f * 4,
                        16.0f * 4
                    );
                    break;
            }

            registry.AssignComponent(
                result,
                new Comps.Shapes.Rectangle {
                    data = rectangle
                }
            );

            return result;
        }

    }
}
