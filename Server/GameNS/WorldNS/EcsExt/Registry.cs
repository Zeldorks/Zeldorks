using System.Collections.Generic;

using NetGameShared.Ecs;
using Ecs = NetGameShared.Ecs;
using NetProtocol = NetGameShared.Net.Protocol;
using Packets = NetGameShared.Net.Protocol.Packets;

using Tick = System.UInt32;

namespace NetGameServer.GameNS.WorldNS.EcsExt
{
    public class Registry : Ecs.Registry
    {
        // ---
        // Snap methods

        private NetProtocol.EcDict<Comp> SnapEcDict<Comp>()
            where Comp : INetComponent<Comp>, new()
        {
            var result = new NetProtocol.EcDict<Comp>();
            HashSet<Entity> entities = GetEntities(
                typeof(Comp)
            );

            foreach (Entity entity in entities) {
                var comp = GetComponentUnsafe<Comp>(entity);

                // We clone the component because we need the snapshot and
                // registry to have completely separate memory.
                // `Entity` is a struct so there is no need to explicitly clone.
                var compClone = (Comp)comp.Clone();
                result.data.Add(entity, compClone);
            }

            return result;
        }

        public Packets.GameSnapshot Snap(Tick tick)
        {
            return new Packets.GameSnapshot()
            {
                Tick = tick,
                Positions = SnapEcDict<Ecs.Components.Position>(),
                ShapeRectangles = SnapEcDict<Ecs.Components.Shapes.Rectangle>(),
                Characters = SnapEcDict<Ecs.Components.Character>(),
                Projectiles = SnapEcDict<Ecs.Components.Projectile>(),
                Items = SnapEcDict<Ecs.Components.Item>(),
                OrientationCardinals = SnapEcDict<Ecs.Components.Orientations.Cardinal>(),
                Playables = SnapEcDict<Ecs.Components.Playable>(),
                Doors = SnapEcDict<Ecs.Components.Door>(), 
                Inventories = SnapEcDict<Ecs.Components.Inventory>()
            };
        }
    }
}
