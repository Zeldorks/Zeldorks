using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static NetGameClient.Util.Drawing;
using Ecs = NetGameShared.Ecs;
using PositionComp = NetGameShared.Ecs.Components.Position;
using SpriteComp = NetGameClient.GameNS.WorldNS.EcsExt.Components.Visibles.Sprite;
using static NetGameShared.Constants.Server.Timing;
using static NetGameClient.GameNs.WorldNS.EcsExt.Systems.SpriteAnimation;

using DrawingPosRectangle = Microsoft.Xna.Framework.Rectangle;
using DrawingVector = Microsoft.Xna.Framework.Point;

using Tick = System.UInt32;

namespace NetGameClient.GameNS.WorldNS.EcsExt.Systems
{
    public static class SpriteRender
    {

        public static void Render(
            SpriteComp spriteComp,
            PositionComp positionComp,
            SpriteBatch spriteBatch,
            Camera camera
        ) {
            // t = Global.tick;
            int currentframe = DateTime.Now.Millisecond;

            //int linkframe = (int)positionComp.data.X % 2;
            var drawingPos =
                 camera.GetDrawingPosition(positionComp.data) -
                 spriteComp.destOffset;

            DrawingPosRectangle destPosRectangle = GetCenteredDrawingPosRectangle(
                drawingPos,
                camera.Scale(spriteComp.destRectangle)
            );

            int x = (int)currentframe / 20;
            //want all sprites except items
            if (spriteComp.texture.Name != "Sprites/Items")
            {
                if(x  >0 && x <17)
                {
                
                    if (spriteComp.texturePosRectangle.Width == 32) { spriteComp.texturePosRectangle.X += 32; }
                    else if (spriteComp.texturePosRectangle.Width == 16) { spriteComp.texturePosRectangle.X += 16; }
                    
                }

            }else if (spriteComp.texture.Name == "Sprites/Items")
            {
                int y = (int)currentframe / 20;
                //chech if sprite is boomerang
                if(spriteComp.texturePosRectangle.X == 36 && spriteComp.texturePosRectangle.Y == 19)
                {
                    if(y > 0 && y < 8)
                    {
                        spriteComp.texturePosRectangle.X += 16;
                    }
                    if(y > 8 && y < 14)
                    {
                        spriteComp.texturePosRectangle.X += 32;
                    }

                }
                //check if sprite is rupee
                if (spriteComp.texturePosRectangle.X == 132 && spriteComp.texturePosRectangle.Y == 0)
                {
                    if (x > 0 && x < 17)
                    {
                        spriteComp.texturePosRectangle.X += spriteComp.texturePosRectangle.Width;
                    }
                }
                //check if sprite is heart
                if(spriteComp.texturePosRectangle.X == 16*6 && spriteComp.texturePosRectangle.Y == 0)
                {
                    spriteComp.texturePosRectangle.X += spriteComp.texturePosRectangle.Width;
                }

            }
            //spriteComp.texturePosRectangle.X += Animate(spriteComp);

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
        public static void Render(
            SpriteComp spriteComp,
            PositionComp positionComp,
            SpriteBatch spriteBatch
            )
        {
            var drawingPos = new DrawingVector((int)positionComp.data.X, (int)positionComp.data.Y);

            DrawingPosRectangle destPosRectangle = GetCenteredDrawingPosRectangle(
                drawingPos,
                spriteComp.destRectangle
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

                Render(spriteComp, positionComp, spriteBatch, camera);
            }
        }
    }
}
