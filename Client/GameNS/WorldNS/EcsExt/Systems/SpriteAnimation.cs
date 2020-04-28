using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static NetGameClient.Util.Drawing;
using Ecs = NetGameShared.Ecs;
using PositionComp = NetGameShared.Ecs.Components.Position;
using SpriteComp = NetGameClient.GameNS.WorldNS.EcsExt.Components.Visibles.Sprite;
using static NetGameShared.Constants.Server.Timing;

using DrawingPosRectangle = Microsoft.Xna.Framework.Rectangle;

using Tick = System.UInt32;

namespace NetGameClient.GameNs.WorldNS.EcsExt.Systems
{
    class SpriteAnimation
    {
        public struct PrevFrame
        {
            public static int timeSinceLastFrame = 0;
            public static int millisecondsPerFrame = 50;
        }

        public void Animate(SpriteComp spriteComp)
        {
            //want to change it so it just returns the new x component
            int currentframe = (int)(DateTime.Now.Millisecond);
            int x = currentframe % 3;

            if (spriteComp.texture.Name != "Sprites/Items")
            {
                //if (PrevFrame.timeSinceLastFrame > PrevFrame.millisecondsPerFrame)
                if (x == 1)
                {
                    //PrevFrame.timeSinceLastFrame -= PrevFrame.millisecondsPerFrame;

                    if (spriteComp.texturePosRectangle.Width == 32) { spriteComp.texturePosRectangle.X += 32; }
                    else if (spriteComp.texturePosRectangle.Width == 16) { spriteComp.texturePosRectangle.X += 16; }

                }

            }
            else if (spriteComp.texture.Name == "Sprites/Items")
            {
                if (spriteComp.texturePosRectangle.X == 36 && spriteComp.texturePosRectangle.Y == 19)
                {
                    x = currentframe % 3;
                    if (x == 1)
                    {
                        spriteComp.texturePosRectangle.X += spriteComp.texturePosRectangle.Width;
                    }
                    if (x == 2)
                    {
                        spriteComp.texturePosRectangle.X += spriteComp.texturePosRectangle.Width;
                    }

                }
                if (spriteComp.texturePosRectangle.X == 132 && spriteComp.texturePosRectangle.Y == 0)
                {
                    x = currentframe % 3;
                    if (x == 1)
                    {
                        spriteComp.texturePosRectangle.X += spriteComp.texturePosRectangle.Width;
                    }
                }

            }


        }
    }
}
