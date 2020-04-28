using System;
using PhysicalVector2 = Microsoft.Xna.Framework.Vector2;

namespace NetGameShared.Util
{
    public static class RandomExt
    {
        public static double NextDouble(
            this Random random,
            double min,
            double max
        ) {
            double range = max - min;
            return (random.NextDouble() * range) + min;
        }

        public static PhysicalVector2 NextPhysicalVector2(this Random random)
        {
            var result = new PhysicalVector2(
                (float)random.NextDouble(-1.0, 1.0),
                (float)random.NextDouble(-1.0, 1.0)
            );
            result.Normalize();
            return result;
        }
    }
}
