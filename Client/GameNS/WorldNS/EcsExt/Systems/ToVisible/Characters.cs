using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Ecs = NetGameShared.Ecs;

using Comps = NetGameShared.Ecs.Components;
using CompsExt = NetGameClient.GameNS.WorldNS.EcsExt.Components;

using NetGameClient.Util;
using Physical = NetGameShared.Util.Physical;
using DrawingPosRectangle = Microsoft.Xna.Framework.Rectangle;
using DrawingVector = Microsoft.Xna.Framework.Point;

namespace NetGameClient.GameNS.WorldNS.EcsExt.Systems.ToVisible
{
    public static class Characters
    {
        private static CompsExt.Visibles.Sprite CreateVisibleComp(
            Comps.Character characterComp,
            Comps.Orientations.Cardinal orientationComp,
            ContentManager contentManager
        ) {
            Texture2D linkSheet = contentManager.Load<Texture2D>("Sprites/Link");
            switch (characterComp.kind) {
                case Comps.Character.Kind.OrangeLink:
                    linkSheet = contentManager.Load<Texture2D>("Sprites/OrangeLink");
                    goto case Comps.Character.Kind.Link;
                case Comps.Character.Kind.PinkLink:
                    linkSheet = contentManager.Load<Texture2D>("Sprites/PinkLink");
                    goto case Comps.Character.Kind.Link;
                case Comps.Character.Kind.BlackLink:
                    linkSheet = contentManager.Load<Texture2D>("Sprites/BlackLink");
                    goto case Comps.Character.Kind.Link;
                case Comps.Character.Kind.BlueLink:
                    linkSheet = contentManager.Load<Texture2D>("Sprites/BlueLink");
                    goto case Comps.Character.Kind.Link;
                case Comps.Character.Kind.PurpleLink:
                    linkSheet = contentManager.Load<Texture2D>("Sprites/PurpleLink");
                    goto case Comps.Character.Kind.Link;
                case Comps.Character.Kind.RedLink:
                    linkSheet = contentManager.Load<Texture2D>("Sprites/RedLink");
                    goto case Comps.Character.Kind.Link;
                case Comps.Character.Kind.TealLink:
                    linkSheet = contentManager.Load<Texture2D>("Sprites/TealLink");
                    goto case Comps.Character.Kind.Link;
                case Comps.Character.Kind.YellowLink:
                    linkSheet = contentManager.Load<Texture2D>("Sprites/YellowLink");
                    goto case Comps.Character.Kind.Link;
                case Comps.Character.Kind.WhiteLink:
                    linkSheet = contentManager.Load<Texture2D>("Sprites/WhiteLink");
                    goto case Comps.Character.Kind.Link;
                case Comps.Character.Kind.Link:
                    switch (orientationComp.data) {
                        case Physical.Orientation.Cardinal.Up:
                            if (characterComp.attacking) {
                                return new CompsExt.Visibles.Sprite {
                                    texture = linkSheet,
                                    texturePosRectangle = new DrawingPosRectangle(16 * 1, 16 * 3, 16, 16 * 2),
                                    destRectangle = new Drawing.Rectangle(16 * 4, 16 * 8),
                                    destOffset = new DrawingVector(0, 16) // TODO: Why does this value work?
                                };
                            } else {
                                return new CompsExt.Visibles.Sprite {
                                    texture = linkSheet,
                                    texturePosRectangle = new DrawingPosRectangle(32, 32, 16, 16),
                                    destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                                };
                            }
                        case Physical.Orientation.Cardinal.Left:
                            if (characterComp.attacking) {
                                return new CompsExt.Visibles.Sprite {
                                    texture = linkSheet,
                                    texturePosRectangle = new DrawingPosRectangle(16 * 2, 16, 16 * 2, 16),
                                    destRectangle = new Drawing.Rectangle(16 * 8, 16 * 4),
                                    destOffset = new DrawingVector(16, 0) // TODO: Why does this value work?
                                };
                            } else {
                                return new CompsExt.Visibles.Sprite {
                                    texture = linkSheet,
                                    texturePosRectangle = new DrawingPosRectangle(8 * 16, 16, 16, 16),
                                    destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                                };
                            }
                        case Physical.Orientation.Cardinal.Down:
                            if (characterComp.attacking) {
                                return new CompsExt.Visibles.Sprite {
                                    texture = linkSheet,
                                    texturePosRectangle = new DrawingPosRectangle(16 * 7, 16 * 2, 16, 16 * 2),
                                    destRectangle = new Drawing.Rectangle(16 * 4, 16 * 8),
                                    destOffset = new DrawingVector(0, -16) // TODO: Why does this value work?
                                };
                            } else {
                                return new CompsExt.Visibles.Sprite {
                                    texture = linkSheet,
                                    texturePosRectangle = new DrawingPosRectangle(0, 32, 16, 16),
                                    destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                                };
                            }
                        case Physical.Orientation.Cardinal.Right:
                            if (characterComp.attacking) {
                                return new CompsExt.Visibles.Sprite {
                                    texture = linkSheet,
                                    texturePosRectangle = new DrawingPosRectangle(16 * 4, 0, 16 * 2, 16),
                                    destRectangle = new Drawing.Rectangle(16 * 8, 16 * 4),
                                    destOffset = new DrawingVector(-16, 0) // TODO: Why does this value work?
                                };
                            } else {
                                return new CompsExt.Visibles.Sprite {
                                    texture = linkSheet,
                                    texturePosRectangle = new DrawingPosRectangle(8 * 16, 0, 16, 16),
                                    destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                                };
                            }
                        default:
                            throw new ArgumentException(
                                "Invalid orientation direction"
                            );
                    }
                case Comps.Character.Kind.Aquamentus:
                    switch (orientationComp.data) {
                        case Physical.Orientation.Cardinal.Left:
                            return new CompsExt.Visibles.Sprite {
                                texture = contentManager.Load<Texture2D>("Sprites/Enemies"),
                                texturePosRectangle = new DrawingPosRectangle(0, 0, 32, 32),
                                destRectangle = new Drawing.Rectangle(32 * 4, 32 * 4)
                            };
                        case Physical.Orientation.Cardinal.Right:
                            return new CompsExt.Visibles.Sprite {
                                texture = contentManager.Load<Texture2D>("Sprites/Enemies"),
                                texturePosRectangle = new DrawingPosRectangle(5, 32, 30, 32),
                                destRectangle = new Drawing.Rectangle(32 * 4, 32 * 4)
                            };
                        default:
                            throw new ArgumentException(
                                "Invalid orientation direction"
                            );
                    }
                case Comps.Character.Kind.Zol:
                    return new CompsExt.Visibles.Sprite {
                        texture = contentManager.Load<Texture2D>("Sprites/Enemies"),
                        texturePosRectangle = new DrawingPosRectangle(96, 32, 16, 16),
                        destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                    };
                case Comps.Character.Kind.Goriya:
                    switch (orientationComp.data)
                    {
                        case Physical.Orientation.Cardinal.Up:
                            return new CompsExt.Visibles.Sprite
                            {
                                texture = contentManager.Load<Texture2D>("Sprites/Enemies"),
                                texturePosRectangle = new DrawingPosRectangle(32, 64, 16, 16),
                                destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                            };
                        case Physical.Orientation.Cardinal.Left:
                            return new CompsExt.Visibles.Sprite
                            {
                                texture = contentManager.Load<Texture2D>("Sprites/Enemies"),
                                texturePosRectangle = new DrawingPosRectangle(96, 64, 16, 16),
                                destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                            };
                        case Physical.Orientation.Cardinal.Down:
                            return new CompsExt.Visibles.Sprite
                            {
                                texture = contentManager.Load<Texture2D>("Sprites/Enemies"),
                                texturePosRectangle = new DrawingPosRectangle(0, 64, 16, 16),
                                destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                            };
                        case Physical.Orientation.Cardinal.Right:
                            return new CompsExt.Visibles.Sprite
                            {
                                texture = contentManager.Load<Texture2D>("Sprites/Enemies"),
                                texturePosRectangle = new DrawingPosRectangle(64, 64, 16, 16),
                                destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                            };
                        default:
                            throw new ArgumentException(
                                "Invalid orientation direction"
                            );
                    }
                case Comps.Character.Kind.Dodongos:
                    switch (orientationComp.data)
                    {
                        case Physical.Orientation.Cardinal.Up:
                            return new CompsExt.Visibles.Sprite
                            {
                                texture = contentManager.Load<Texture2D>("Sprites/Enemies"),
                                texturePosRectangle = new DrawingPosRectangle(64, 96, 16, 16),
                                destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                            };
                        case Physical.Orientation.Cardinal.Left:
                            return new CompsExt.Visibles.Sprite
                            {
                                texture = contentManager.Load<Texture2D>("Sprites/Enemies"),
                                texturePosRectangle = new DrawingPosRectangle(64, 80, 32, 16),
                                destRectangle = new Drawing.Rectangle(32 * 4, 16 * 4)
                            };
                        case Physical.Orientation.Cardinal.Down:
                            return new CompsExt.Visibles.Sprite
                            {
                                texture = contentManager.Load<Texture2D>("Sprites/Enemies"),
                                texturePosRectangle = new DrawingPosRectangle(128, 80, 16, 16),
                                destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                            };
                        case Physical.Orientation.Cardinal.Right:
                            return new CompsExt.Visibles.Sprite
                            {
                                texture = contentManager.Load<Texture2D>("Sprites/Enemies"),
                                texturePosRectangle = new DrawingPosRectangle(0, 80, 32, 16),
                                destRectangle = new Drawing.Rectangle(32 * 4, 16 * 4)
                            };
                        default:
                            throw new ArgumentException(
                                "Invalid orientation direction"
                            );
                    }
                case Comps.Character.Kind.Stalfos:
                    return new CompsExt.Visibles.Sprite
                    {
                        texture = contentManager.Load<Texture2D>("Sprites/Enemies"),
                        texturePosRectangle = new DrawingPosRectangle(128, 64, 16, 16),
                        destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                    };
                case Comps.Character.Kind.Rope:
                    switch (orientationComp.data)
                    {
                        case Physical.Orientation.Cardinal.Right:
                            return new CompsExt.Visibles.Sprite
                            {
                                texture = contentManager.Load<Texture2D>("Sprites/Enemies"),
                                texturePosRectangle = new DrawingPosRectangle(0, 112, 16, 16),
                                destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                            };
                        case Physical.Orientation.Cardinal.Left:
                            return new CompsExt.Visibles.Sprite
                            {
                                texture = contentManager.Load<Texture2D>("Sprites/Enemies"),
                                texturePosRectangle = new DrawingPosRectangle(32, 112, 16, 16),
                                destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                            };
                        default:
                            throw new ArgumentException(
                                "Invalid orientation direction"
                            );
                    }
                case Comps.Character.Kind.Keese:
                    return new CompsExt.Visibles.Sprite
                    {
                        texture = contentManager.Load<Texture2D>("Sprites/Enemies"),
                        texturePosRectangle = new DrawingPosRectangle(128, 96, 16, 16),
                        destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                    };
                case Comps.Character.Kind.Wallmaster:
                    return new CompsExt.Visibles.Sprite
                    {
                        texture = contentManager.Load<Texture2D>("Sprites/Enemies"),
                        texturePosRectangle = new DrawingPosRectangle(64, 112, 16, 16),
                        destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                    };
                default:
                    throw new ArgumentException("Invalid character kind");
            }
        }

        public static void Run(
            Ecs.Registry registry,
            ContentManager contentManager
        ) {
            HashSet<Ecs.Entity> entities = registry.GetEntities(
                typeof(Comps.Character),
                typeof(Comps.Orientations.Cardinal)
            );

            foreach (Ecs.Entity entity in entities) {
                var characterComp = registry
                    .GetComponentUnsafe<Comps.Character>(entity);

                var orientationComp = registry
                    .GetComponentUnsafe<Comps.Orientations.Cardinal>(entity);

                CompsExt.Visibles.Sprite sprite = CreateVisibleComp(
                    characterComp,
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
