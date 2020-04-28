using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Ecs = NetGameShared.Ecs;

using ItemComp = NetGameShared.Ecs.Components.Item;
using ItemKind = NetGameShared.Ecs.Components.Item.Kind;

using VisibleComp = NetGameClient.GameNS.WorldNS.EcsExt.Components.Visibles.Sprite;

using NetGameClient.Util;
using DrawingPosRectangle = Microsoft.Xna.Framework.Rectangle;

namespace NetGameClient.GameNS.WorldNS.EcsExt.Systems.ToVisible
{
    public class Items
    {
        private static VisibleComp CreateVisibleComp(
            ItemComp itemComp,
            ContentManager contentManager
        ) {
            switch (itemComp.kind) {
                case ItemKind.Compass:
                    return new VisibleComp {
                        texture = contentManager.Load<Texture2D>("Sprites/Items"),
                        texturePosRectangle = new DrawingPosRectangle(0, 0, 16, 16),
                        destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                    };
                case ItemKind.Heart:
                    return new VisibleComp {
                        texture = contentManager.Load<Texture2D>("Sprites/Items"),
                        texturePosRectangle = new DrawingPosRectangle(16 * 3, 0, 16, 16),
                        destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                    };
                case ItemKind.Bomb:
                    return new VisibleComp
                    {
                        texture = contentManager.Load<Texture2D>("Sprites/Items"),
                        texturePosRectangle = new DrawingPosRectangle(130, 18, 14, 14),
                        destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                    };
                case ItemKind.Boomerang:
                    return new VisibleComp
                    {
                        texture = contentManager.Load<Texture2D>("Sprites/Items"),
                        texturePosRectangle = new DrawingPosRectangle(36, 19, 11, 11),
                        destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                    };
                case ItemKind.Clock:
                    return new VisibleComp
                    {
                        texture = contentManager.Load<Texture2D>("Sprites/Items"),
                        texturePosRectangle = new DrawingPosRectangle(66, 0, 16, 16),
                        destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                    };
                case ItemKind.Bow:
                    return new VisibleComp
                    {
                        texture = contentManager.Load<Texture2D>("Sprites/Items"),
                        texturePosRectangle = new DrawingPosRectangle(84, 0, 16, 16),
                        destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                    };
                case ItemKind.Rupee:
                    return new VisibleComp
                    {
                        texture = contentManager.Load<Texture2D>("Sprites/Items"),
                        texturePosRectangle = new DrawingPosRectangle(132, 0, 16, 16),
                        destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                    };
                case ItemKind.TriforceShard:
                    return new VisibleComp
                    {
                    texture = contentManager.Load<Texture2D>("Sprites/Items"),
                        texturePosRectangle = new DrawingPosRectangle(3, 19, 11, 11),
                        destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                    };
                case ItemKind.Candle:
                    return new VisibleComp
                    {
                        texture = contentManager.Load<Texture2D>("Sprites/Items"),
                        texturePosRectangle = new DrawingPosRectangle(112, 32, 16, 16),
                        destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                    };
                case ItemKind.Map:
                    return new VisibleComp
                    {
                        texture = contentManager.Load<Texture2D>("Sprites/Items"),
                        texturePosRectangle = new DrawingPosRectangle(20, 0, 16, 16),
                        destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                    };
                case ItemKind.RedKey:
                    return new VisibleComp
                    {
                        texture = contentManager.Load<Texture2D>("Sprites/Items"),
                        texturePosRectangle = new DrawingPosRectangle(16 * 4, 16 * 3, 16, 16),
                        destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                    };

                case ItemKind.BlueKey:
                    return new VisibleComp
                    {
                        texture = contentManager.Load<Texture2D>("Sprites/Items"),
                        texturePosRectangle = new DrawingPosRectangle(16 * 5, 16 * 3, 16, 16),
                        destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                    };

                case ItemKind.GreenKey:
                    return new VisibleComp
                    {
                        texture = contentManager.Load<Texture2D>("Sprites/Items"),
                        texturePosRectangle = new DrawingPosRectangle(16 * 6, 16 * 3, 16, 16),
                        destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                    };

                case ItemKind.PurpleKey:
                    return new VisibleComp
                    {
                        texture = contentManager.Load<Texture2D>("Sprites/Items"),
                        texturePosRectangle = new DrawingPosRectangle(16 * 7, 16 * 3, 16, 16),
                        destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                    };

                case ItemKind.YellowKey:
                    return new VisibleComp
                    {
                        texture = contentManager.Load<Texture2D>("Sprites/Items"),
                        texturePosRectangle = new DrawingPosRectangle(16 * 3, 16 * 3, 16, 16),
                        destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
                    };
                case ItemKind.Sword:
                    return new VisibleComp
                    {
                        texture = contentManager.Load<Texture2D>("Sprites/Items"),
                        texturePosRectangle = new DrawingPosRectangle(16 * 4, 16 * 2, 16, 16),
                        destRectangle = new Drawing.Rectangle(16 * 4, 16 * 4)
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
                typeof(ItemComp)
            );

            foreach (Ecs.Entity entity in entities) {
                var itemComp = registry
                    .GetComponentUnsafe<ItemComp>(entity);

                VisibleComp sprite = CreateVisibleComp(
                    itemComp,
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
