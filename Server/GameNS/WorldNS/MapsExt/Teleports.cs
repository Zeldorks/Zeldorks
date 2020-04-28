using TiledSharp;
using System.Collections.Generic;

using PhysicalVector2 = Microsoft.Xna.Framework.Vector2;

namespace NetGameServer.GameNS.WorldNS.MapsExt
{
    public class Teleports
    {
        public PhysicalVector2 triforceDestination;

        // Pre-condition: `teleportGroup` is the object group containing teleports
        public Teleports(TmxObjectGroup teleportGroup)
        {
            foreach (TmxObject tmxObject in teleportGroup.Objects) {
                switch (tmxObject.Type) {
                    case "TriforceDestination":
                        var position = new PhysicalVector2(
                            (float)tmxObject.X,
                            (float)tmxObject.Y
                        );
                        triforceDestination = position;
                        break;
                    default:
                        System.Console.WriteLine("Invalid spawn kind");
                        break;
                }
            }
        }
    }
}
