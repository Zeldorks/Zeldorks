using System;
using System.Collections.Generic;

using NetGameShared.Ecs;
using Ecs = NetGameShared.Ecs;
using NetProtocol = NetGameShared.Net.Protocol;
using Packets = NetGameShared.Net.Protocol.Packets;

namespace NetGameClient.GameNS.WorldNS.EcsExt
{
    public class Registry : Ecs.Registry
    {
        // For the client, an entity is considered empty if that entity no
        // longer exists on the server. In other words, the server has removed
        // all components belonging to the entity. Since the client may have
        // extra components that the server does not know about, we need to
        // delete hollow entities so those extra components do not stay in the
        // registry.
        protected override void RemoveIfEmpty(Entity entity)
        {
            if (data.ContainsKey(entity)) {
                var compTypes = new HashSet<Type>(data[entity].Keys);
                if (!compTypes.Contains(typeof(Ecs.Components.Position)) &&
                    !compTypes.Contains(typeof(Ecs.Components.Shapes.Rectangle)) &&
                    !compTypes.Contains(typeof(Ecs.Components.Character)) &&
                    !compTypes.Contains(typeof(Ecs.Components.Projectile)) &&
                    !compTypes.Contains(typeof(Ecs.Components.Item)) &&
                    !compTypes.Contains(typeof(Ecs.Components.Door)) &&
                    !compTypes.Contains(typeof(Ecs.Components.Orientations.Cardinal)) &&
                    !compTypes.Contains(typeof(Ecs.Components.Playable)) &&
                    !compTypes.Contains(typeof(Ecs.Components.Inventory))
                ) {
                    Remove(entity);
                }
            }
        }

        // ---
        // Process methods

        public void ProcessEcDict<Comp>(
            NetProtocol.EcDict<Comp> ecDict
        ) where Comp : INetComponent<Comp>, new()
        {
            foreach (KeyValuePair<Entity, Comp> pair in ecDict.data) {
                Entity entity = pair.Key;
                Comp comp = pair.Value;

                // We clone the component because we need the snapshot and
                // registry to have completely separate memory.
                // `Entity` is a struct so there is no need to explicitly clone.
                Comp compClone = (Comp)comp.Clone();
                AssignComponent(
                    entity,
                    compClone
                );
            }
        }

        public void Process(Packets.GameSnapshot gameSnapshot)
        {
            Clear();
            ProcessEcDict(gameSnapshot.Positions);
            ProcessEcDict(gameSnapshot.ShapeRectangles);
            ProcessEcDict(gameSnapshot.Characters);
            ProcessEcDict(gameSnapshot.Projectiles);
            ProcessEcDict(gameSnapshot.Items);
            ProcessEcDict(gameSnapshot.Doors);
            ProcessEcDict(gameSnapshot.OrientationCardinals);
            ProcessEcDict(gameSnapshot.Playables);
            ProcessEcDict(gameSnapshot.Inventories);
        }

        // ---
        // Process methods (with delta)

        public void ProcessEcDictDelta<Comp>(
            NetProtocol.EcDictDelta<Comp> ecDictDelta
        ) where Comp : INetComponent<Comp>, new()
        {
            foreach (KeyValuePair<Entity, Comp> pair in ecDictDelta.toAdd) {
                Entity entity = pair.Key;
                Comp comp = pair.Value;
                Comp compClone = (Comp)comp.Clone();
                AssignComponent(
                    entity,
                    compClone
                );
            }

            // Adding a component will overwrite any old component of the same
            // type belonging to the entity
            foreach (KeyValuePair<Entity, Comp> pair in ecDictDelta.toUpdate) {
                Entity entity = pair.Key;
                Comp comp = pair.Value;
                Comp compClone = (Comp)comp.Clone();
                AssignComponent(
                    entity,
                    compClone
                );
            }

            foreach (Entity entity in ecDictDelta.toRemove) {
                RemoveComponent(
                    entity,
                    typeof(Comp)
                );
            }
        }

        public void Process(Packets.GameSnapshotDelta gameSnapshotDelta)
        {
            ProcessEcDictDelta(gameSnapshotDelta.Positions);
            ProcessEcDictDelta(gameSnapshotDelta.ShapeRectangles);
            ProcessEcDictDelta(gameSnapshotDelta.Characters);
            ProcessEcDictDelta(gameSnapshotDelta.Projectiles);
            ProcessEcDictDelta(gameSnapshotDelta.Items);
            ProcessEcDictDelta(gameSnapshotDelta.Doors);
            ProcessEcDictDelta(gameSnapshotDelta.OrientationCardinals);
            ProcessEcDictDelta(gameSnapshotDelta.Playables);
            ProcessEcDictDelta(gameSnapshotDelta.Inventories);
        }
    }
}
