using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Ecs = NetGameShared.Ecs;

using Comps = NetGameShared.Ecs.Components;
using CompsExt = NetGameClient.GameNS.WorldNS.EcsExt.Components;

using NetGameClient.Util;
using DrawingPosRectangle = Microsoft.Xna.Framework.Rectangle;

namespace NetGameClient.GameNS.WorldNS.EcsExt.Systems.ToVisible
{
    public class Doors
    {
        private static CompsExt.Visibles.Sprite CreateVisibleComp(
            Comps.Door doorComp,
            ContentManager contentManager
        ) {
            switch (doorComp.kind) {
                case Comps.Door.Kind.Red:
                    return new CompsExt.Visibles.Sprite
                    {
                        texture = contentManager.Load<Texture2D>("MapImages/Doors"),
                        texturePosRectangle = new DrawingPosRectangle(64, 0, 32*6, 32*2),
                        destRectangle = new Drawing.Rectangle(16 * 4*3, 16 * 4)
                    };

                case Comps.Door.Kind.Blue:
                    return new CompsExt.Visibles.Sprite
                    {
                        texture = contentManager.Load<Texture2D>("MapImages/Doors"),
                        texturePosRectangle = new DrawingPosRectangle(64, 64, 32 * 6, 32 * 2),
                        destRectangle = new Drawing.Rectangle(16 * 4 * 3, 16 * 4)
                    };

                case Comps.Door.Kind.Green:
                    return new CompsExt.Visibles.Sprite
                    {
                        texture = contentManager.Load<Texture2D>("MapImages/Doors"),
                        texturePosRectangle = new DrawingPosRectangle(64, 64*2, 32 * 6, 32 * 2),
                        destRectangle = new Drawing.Rectangle(16 * 4 * 3, 16 * 4)
                    };

                case Comps.Door.Kind.Purple:
                    return new CompsExt.Visibles.Sprite
                    {
                        texture = contentManager.Load<Texture2D>("MapImages/Doors"),
                        texturePosRectangle = new DrawingPosRectangle(64, 64*3, 32 * 6, 32 * 2),
                        destRectangle = new Drawing.Rectangle(16 * 4 * 3, 16 * 4)
                    };

                case Comps.Door.Kind.Yellow:
                    return new CompsExt.Visibles.Sprite
                    {
                        texture = contentManager.Load<Texture2D>("MapImages/Doors"),
                        texturePosRectangle = new DrawingPosRectangle(64, 64*4, 32 * 6, 32 * 2),
                        destRectangle = new Drawing.Rectangle(16 * 4 * 3, 16 * 4)
                    };
                default:
                    throw new NotImplementedException("TODO");
            }
        }

        public static void Run(
            Ecs.Registry registry,
            ContentManager contentManager
        ) {
            HashSet<Ecs.Entity> entities = registry.GetEntities(
                typeof(Comps.Door)
            );

            foreach (Ecs.Entity entity in entities) {
                var obstacleComp = registry
                    .GetComponentUnsafe<Comps.Door>(entity);

                CompsExt.Visibles.Sprite sprite = CreateVisibleComp(
                    obstacleComp,
                    contentManager
                );

                registry.AssignComponent(
                    entity,
                    sprite
                );
            }
        }
    }
}
