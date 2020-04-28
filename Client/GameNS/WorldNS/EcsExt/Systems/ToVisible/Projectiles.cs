using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Ecs = NetGameShared.Ecs;

using Comps = NetGameShared.Ecs.Components;
using CompsExt = NetGameClient.GameNS.WorldNS.EcsExt.Components;

using NetGameClient.Util;
using Physical = NetGameShared.Util.Physical;
using DrawingVector = Microsoft.Xna.Framework.Point;
using DrawingPosRectangle = Microsoft.Xna.Framework.Rectangle;

namespace NetGameClient.GameNS.WorldNS.EcsExt.Systems.ToVisible
{
    public class Projectiles
    {
        private static bool IsVisible(Comps.Projectile.Kind projectileKind)
        {
            switch (projectileKind) {
                case Comps.Projectile.Kind.SwordSlash:
                    return false;
                default:
                    return true;
            }
        }

        private static CompsExt.Visibles.Sprite CreateVisibleComp(
            Comps.Projectile projectileComp,
            Comps.Orientations.Cardinal orientationComp,
            ContentManager contentManager
        ) {
            switch (projectileComp.kind) {
                case Comps.Projectile.Kind.Arrow:
                    switch (orientationComp.data)
                    {
                        case Physical.Orientation.Cardinal.Up:
                            return new CompsExt.Visibles.Sprite
                            {
                                texture = contentManager.Load<Texture2D>("Sprites/Items"),
                                texturePosRectangle = new DrawingPosRectangle(16, 32, 16, 16),
                                destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                            };
                        case Physical.Orientation.Cardinal.Down:
                            return new CompsExt.Visibles.Sprite
                            {
                                texture = contentManager.Load<Texture2D>("Sprites/Items"),
                                texturePosRectangle = new DrawingPosRectangle(16, 32, 16, 16),
                                destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4),
                                effects = SpriteEffects.FlipVertically
                            };
                        case Physical.Orientation.Cardinal.Right:
                            return new CompsExt.Visibles.Sprite
                            {
                                texture = contentManager.Load<Texture2D>("Sprites/Items"),
                                texturePosRectangle = new DrawingPosRectangle(16, 32, 16, 16),
                                destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4),
                                rotation = (float)Math.PI / 2,
                                destOffset = new DrawingVector(-16 * 4, 0)
                            };
                        case Physical.Orientation.Cardinal.Left:
                            return new CompsExt.Visibles.Sprite
                            {
                                texture = contentManager.Load<Texture2D>("Sprites/Items"),
                                texturePosRectangle = new DrawingPosRectangle(16, 32, 16, 16),
                                destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4),
                                rotation = (float)Math.PI / 2,
                                destOffset = new DrawingVector(-16 * 4, 0),
                                effects = SpriteEffects.FlipVertically
                            };
                        default:
                            throw new ArgumentException("Invalid orientation direction");
                    }
                case Comps.Projectile.Kind.Bomb:
                    return new CompsExt.Visibles.Sprite
                    {
                        texture = contentManager.Load<Texture2D>("Sprites/Items"),
                        texturePosRectangle = new DrawingPosRectangle(130, 18, 14, 14),
                        destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                    };
                case Comps.Projectile.Kind.Boomerang:
                    return new CompsExt.Visibles.Sprite
                    {
                        texture = contentManager.Load<Texture2D>("Sprites/Items"),
                        texturePosRectangle = new DrawingPosRectangle(36, 19, 11, 11),
                        destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                    };
                case Comps.Projectile.Kind.RedKey:
                    return new CompsExt.Visibles.Sprite
                    {
                        texture = contentManager.Load<Texture2D>("Sprites/Items"),
                        texturePosRectangle = new DrawingPosRectangle(28, 0, 16, 16),
                        destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                    };
                case Comps.Projectile.Kind.BlueKey:
                    return new CompsExt.Visibles.Sprite
                    {
                        texture = contentManager.Load<Texture2D>("Sprites/Items"),
                        texturePosRectangle = new DrawingPosRectangle(28, 0, 16, 16),
                        destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                    };
                case Comps.Projectile.Kind.GreenKey:
                    return new CompsExt.Visibles.Sprite
                    {
                        texture = contentManager.Load<Texture2D>("Sprites/Items"),
                        texturePosRectangle = new DrawingPosRectangle(28, 0, 16, 16),
                        destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                    };
                case Comps.Projectile.Kind.PurpleKey:
                    return new CompsExt.Visibles.Sprite
                    {
                        texture = contentManager.Load<Texture2D>("Sprites/Items"),
                        texturePosRectangle = new DrawingPosRectangle(28, 0, 16, 16),
                        destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                    };
                case Comps.Projectile.Kind.YellowKey:
                    return new CompsExt.Visibles.Sprite
                    {
                        texture = contentManager.Load<Texture2D>("Sprites/Items"),
                        texturePosRectangle = new DrawingPosRectangle(28, 0, 16, 16),
                        destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                    };
                case Comps.Projectile.Kind.Fireball:
                    return new CompsExt.Visibles.Sprite
                    {
                        texture = contentManager.Load<Texture2D>("Sprites/Enemies"),
                        texturePosRectangle = new DrawingPosRectangle(100, 51, 10, 10),
                        destRectangle = new Drawing.Rectangle(10 * 4, 10 * 4)
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
                typeof(Comps.Projectile),
                typeof(Comps.Orientations.Cardinal)
            );

            foreach (Ecs.Entity entity in entities) {
                var projectileComp = registry
                    .GetComponentUnsafe<Comps.Projectile>(entity);

                if (!IsVisible(projectileComp.kind)) continue;

                var orientationComp = registry
                    .GetComponentUnsafe<Comps.Orientations.Cardinal>(entity);

                CompsExt.Visibles.Sprite sprite = CreateVisibleComp(
                    projectileComp,
                    orientationComp,
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
