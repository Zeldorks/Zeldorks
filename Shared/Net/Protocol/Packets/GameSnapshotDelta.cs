using System;
using System.Collections.Generic;
using Tick = System.UInt32;

namespace NetGameShared.Net.Protocol.Packets
{
    public class GameSnapshotDelta
    {
        public Tick Tick { get; set; }
        public Tick BaseTick { get; set; }

        // TODO: EcDicts
        public EcDictDelta<Ecs.Components.Position> Positions { get; set; }
        public EcDictDelta<Ecs.Components.Shapes.Rectangle> ShapeRectangles { get; set; }
        public EcDictDelta<Ecs.Components.Character> Characters { get; set; }
        public EcDictDelta<Ecs.Components.Projectile> Projectiles { get; set; }
        public EcDictDelta<Ecs.Components.Item> Items { get; set; }

        public EcDictDelta<Ecs.Components.Door> Doors { get; set; }
        public EcDictDelta<Ecs.Components.Orientations.Cardinal> OrientationCardinals { get; set; }
        public EcDictDelta<Ecs.Components.Playable> Playables { get; set; }
        public EcDictDelta<Ecs.Components.Inventory> Inventories { get; set; }

        // Parameter-less constructor needed by LiteNetLib
        public GameSnapshotDelta()
        {
            // Empty
        }

        public GameSnapshotDelta(
            GameSnapshot currentSnap,
            GameSnapshot baseSnap
        ) {
            Tick = currentSnap.Tick;
            BaseTick = baseSnap.Tick;

            Positions = new EcDictDelta<Ecs.Components.Position>(
                currentSnap.Positions,
                baseSnap.Positions
            );

            ShapeRectangles = new EcDictDelta<Ecs.Components.Shapes.Rectangle>(
                currentSnap.ShapeRectangles,
                baseSnap.ShapeRectangles
            );

            Characters = new EcDictDelta<Ecs.Components.Character>(
                currentSnap.Characters,
                baseSnap.Characters
            );

            Projectiles = new EcDictDelta<Ecs.Components.Projectile>(
                currentSnap.Projectiles,
                baseSnap.Projectiles
            );

            Items = new EcDictDelta<Ecs.Components.Item>(
                currentSnap.Items,
                baseSnap.Items
            );

            Doors = new EcDictDelta<Ecs.Components.Door>(
                currentSnap.Doors,
                baseSnap.Doors
            );

            OrientationCardinals = new EcDictDelta<Ecs.Components.Orientations.Cardinal>(
                currentSnap.OrientationCardinals,
                baseSnap.OrientationCardinals
            );

            Playables = new EcDictDelta<Ecs.Components.Playable>(
                currentSnap.Playables,
                baseSnap.Playables
            );

            Inventories = new EcDictDelta<Ecs.Components.Inventory>(
                currentSnap.Inventories,
                baseSnap.Inventories
            );
        }

        public void DebugPrint<Comp>(
            EcDictDelta<Comp> ecDictDelta
        ) where Comp : Ecs.INetComponent<Comp>, new()
        {
            Console.WriteLine("        To add:");
            foreach (KeyValuePair<Ecs.Entity, Comp> pair in ecDictDelta.toAdd) {
                Ecs.Entity entity = pair.Key;
                Comp comp = pair.Value;
                Console.WriteLine("            Entity ID: {0}", entity.id);
                Console.WriteLine("                Component: {0}", comp);
            }

            Console.WriteLine("        To update:");
            foreach (KeyValuePair<Ecs.Entity, Comp> pair in ecDictDelta.toUpdate) {
                Ecs.Entity entity = pair.Key;
                Comp comp = pair.Value;
                Console.WriteLine("            Entity ID: {0}", entity.id);
                Console.WriteLine("                Component: {0}", comp);
            }

            Console.WriteLine("        To remove:");
            foreach (Ecs.Entity entity in ecDictDelta.toRemove) {
                Console.WriteLine("            Entity ID: {0}", entity.id);
            }
        }

        public void DebugPrint()
        {
            Console.WriteLine("Delta game snapshot");
            Console.WriteLine("    Tick: {0}", Tick);
            Console.WriteLine("    Base tick: {0}", BaseTick);

            Console.WriteLine("    Positions:");
            DebugPrint(Positions);

            Console.WriteLine("    Shape rectangles:");
            DebugPrint(ShapeRectangles);

            Console.WriteLine("    Characters:");
            DebugPrint(Characters);

            Console.WriteLine("    Projectiles:");
            DebugPrint(Projectiles);

            Console.WriteLine("    Items:");
            DebugPrint(Items);

            Console.WriteLine("    Obstacles:");
            DebugPrint(Doors);

            Console.WriteLine("    Orientation cardinals:");
            DebugPrint(OrientationCardinals);

            Console.WriteLine("    Playables:");
            DebugPrint(Playables);

            Console.WriteLine("    Inventories:");
            DebugPrint(Inventories);
        }
    }
}
