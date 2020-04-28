using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static NetGameClient.Util.Drawing;
using Ecs = NetGameShared.Ecs;
using PositionComp = NetGameShared.Ecs.Components.Position;
using SpriteComp = NetGameClient.GameNS.WorldNS.EcsExt.Components.Visibles.Sprite;

using DrawingPosRectangle = Microsoft.Xna.Framework.Rectangle;

namespace NetGameClient.GameNS.WorldNS.EcsExt.Systems
{
    public static class SpriteRender
    {
        // To optimize drawing performance, check that the sprite is in range
        // for the camera before calling this
        public static void Render(
            SpriteComp spriteComp,
            PositionComp positionComp,
            SpriteBatch spriteBatch,
            Camera camera
        ) {
            var drawingPos =
                camera.GetDrawingPosition(positionComp.data) -
                spriteComp.destOffset;

            DrawingPosRectangle destPosRectangle = GetCenteredDrawingPosRectangle(
                drawingPos,
                camera.Scale(spriteComp.destRectangle)
            );

            // TODO: Figure out how to use origin with rotation

            spriteBatch.Draw(
                spriteComp.texture,
                destPosRectangle,
                spriteComp.texturePosRectangle,
                spriteComp.tint,
                spriteComp.rotation,
                new Vector2(0, 0),
                spriteComp.effects,
                1.0f
            );
        }

        public static void Run(
            Ecs.Registry ecsRegistry,
            SpriteBatch spriteBatch,
            Camera camera
        ) {
            HashSet<Ecs.Entity> entities = ecsRegistry.GetEntities(
                typeof(SpriteComp),
                typeof(PositionComp)
            );

            foreach (Ecs.Entity entity in entities) {
                SpriteComp spriteComp = ecsRegistry
                    .GetComponentUnsafe<SpriteComp>(entity);

                PositionComp positionComp = ecsRegistry
                    .GetComponentUnsafe<PositionComp>(entity);

                if (camera.IsInRange(positionComp.data)) {
                    Render(spriteComp, positionComp, spriteBatch, camera);
                }
            }
        }
    }
}
