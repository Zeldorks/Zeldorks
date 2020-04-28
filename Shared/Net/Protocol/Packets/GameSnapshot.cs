using System;
using System.Collections.Generic;
using Tick = System.UInt32;

namespace NetGameShared.Net.Protocol.Packets
{
    public class GameSnapshot
    {
        public Tick Tick { get; set; }
        public EcDict<Ecs.Components.Position> Positions { get; set; }
        public EcDict<Ecs.Components.Shapes.Rectangle> ShapeRectangles { get; set; }
        public EcDict<Ecs.Components.Character> Characters { get; set; }
        public EcDict<Ecs.Components.Projectile> Projectiles { get; set; }
        public EcDict<Ecs.Components.Item> Items { get; set; }
        public EcDict<Ecs.Components.Door> Doors { get; set; }
        public EcDict<Ecs.Components.Orientations.Cardinal> OrientationCardinals { get; set; }
        public EcDict<Ecs.Components.Playable> Playables { get; set; }
        public EcDict<Ecs.Components.Inventory> Inventories { get; set; }

        public void DebugPrint<Comp>(
            EcDict<Comp> ecDict
        ) where Comp : Ecs.INetComponent<Comp>, new()
        {
            foreach (KeyValuePair<Ecs.Entity, Comp> pair in ecDict.data) {
                Ecs.Entity entity = pair.Key;
                Comp comp = pair.Value;
                Console.WriteLine("        Entity ID: {0}", entity.id);
                Console.WriteLine("            Component: {0}", comp);
            }
        }

        public void DebugPrint()
        {
            Console.WriteLine("Game snapshot");
            Console.WriteLine("    Tick: {0}", Tick);

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
