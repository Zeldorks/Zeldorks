using System;
using Microsoft.Xna.Framework.Graphics;
using Optional;
using Ecs = NetGameShared.Ecs;
using Comps = NetGameShared.Ecs.Components;
using PhysicalVector2 = Microsoft.Xna.Framework.Vector2;
using DrawingVector = Microsoft.Xna.Framework.Point;
using NetGameClient.Util;
using Physical = NetGameShared.Util.Physical;

namespace NetGameClient.GameNS.WorldNS
{
    public class Camera
    {
        public PhysicalVector2 position;
        private float zoom = 0.5f;

        private (int width, int height) viewport;

        public void SetViewport(GraphicsDevice graphicsDevice)
        {
            viewport.width = graphicsDevice.Viewport.Width;
            viewport.height = graphicsDevice.Viewport.Height;
        }

        private Physical.Rectangle Viewport
        {
            get {
                return new Physical.Rectangle(
                    viewport.width,
                    viewport.height
                ) / zoom;
            }
        }

        private Physical.Rectangle HalfViewport
        {
            get { return Viewport / 2; }
        }

        private PhysicalVector2 CenterPosition
        {
            get { return position + HalfViewport.ToVector2(); }
        }

        public Physical.PosRectangle Range
        {
            get {
                return new Physical.PosRectangle(CenterPosition, Viewport);
            }
        }

        public bool IsInRange(PhysicalVector2 position)
        {
            const float padding = 32.0f;
            var diff = this.CenterPosition - position;
            return
                Math.Abs(diff.X) <= HalfViewport.Width + padding ||
                Math.Abs(diff.Y) <= HalfViewport.Height + padding;
        }

        private DrawingVector Scale(DrawingVector position)
        {
            return new DrawingVector(
                (int)(position.X * zoom),
                (int)(position.Y * zoom)
            );
        }

        public Drawing.Rectangle Scale(Drawing.Rectangle rectangle)
        {
            return rectangle * zoom;
        }

        public DrawingVector GetDrawingPosition(PhysicalVector2 position)
        {
            var diff = position - this.position;
            var unscaledResult = new DrawingVector((int)diff.X, (int)diff.Y);
            return Scale(unscaledResult);
        }

        public void Follow(PhysicalVector2 target) {
            // Fluid camera movement
            // Reference: https://hookrace.net/blog/writing-a-2d-platform-game-in-nim-with-sdl2/
            var distance = position - target + HalfViewport.ToVector2();
            position -= distance * 0.05f;
        }

        public void Follow(
            Ecs.Entity entity,
            Ecs.Registry ecsRegistry
        ) {
            Option<Comps.Position> positionCompOpt = ecsRegistry
                .GetComponent<Comps.Position>(entity);

            positionCompOpt.MatchSome(positionComp => {
                var target = positionComp.data;
                Follow(target);
            });
        }
    }
}
