using Ecs = NetGameShared.Ecs;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Optional;

using Kind = NetGameShared.Ecs.Components.Item.Kind;

namespace NetGameClient.GameNS.WorldNS.EcsExt.Systems
{
    public static class HUD
    {
        // Item position constants.
        public static readonly Vector2 itemStartPosition = new Vector2(17, 36);
        private static readonly Vector2 itemOffset = new Vector2(36, 0);

        // Heart position constants.
        public static readonly Vector2 heartStartPosition = new Vector2(2, 64);
        // Heart sprite positions.
        public static readonly Rectangle fullHeart = new Rectangle(0, 0, 16, 16);
        public static readonly Rectangle halfHeart = new Rectangle(16, 0, 16, 16);
        public static readonly Rectangle emptyHeart = new Rectangle(32, 0, 16, 16);
        // Rupy position constants.
        public static readonly Vector2 rupyStartPosition = new Vector2(30, 81);

        public const int HudScale = 2;
        
        public static void Run(
            Ecs.Entity playerEntity, 
            Ecs.Registry registry, 
            ContentManager contentManager, 
            SpriteBatch spriteBatch
        ) {
            var playerInventoryOption = 
                registry.GetComponent<Ecs.Components.Inventory>(playerEntity);

            playerInventoryOption.MatchSome(playerInventory => 
                Draw(playerInventory, registry, contentManager, spriteBatch)
            );
        }

        private static void Draw(
            Ecs.Components.Inventory playerInventory, 
            Ecs.Registry registry, 
            ContentManager contentManager, 
            SpriteBatch spriteBatch
        ) {
            Texture2D hud = contentManager.Load<Texture2D>("UI/Hud");

            spriteBatch.Draw(
                hud, 
                new Rectangle(0, 0, hud.Width * HudScale, hud.Height * HudScale), 
                Color.White
            );

            for (var i = 0; i < playerInventory.slots.Count; i++)
            {
                playerInventory.slots[i].MatchSome(slot => 
                    RenderItem(slot, contentManager, spriteBatch, i)
                );
            }

            if (playerInventory.data.ContainsKey(Kind.Rupee))
            {
                SpriteFont zeldaFont = contentManager.Load<SpriteFont>("Fonts/Zelda");
                int rupeeCount = playerInventory.data[Kind.Rupee].Count;
                spriteBatch.DrawString(
                    zeldaFont, 
                    rupeeCount.ToString(), 
                    HudScale * (rupyStartPosition - new Vector2(1, -1)), 
                    Color.Black, 
                    0, 
                    Vector2.Zero, 
                    HudScale, 
                    SpriteEffects.None, 
                    0
                );

                spriteBatch.DrawString(
                    zeldaFont, 
                    rupeeCount.ToString(), 
                    HudScale * rupyStartPosition, 
                    Color.White, 
                    0, 
                    Vector2.Zero, 
                    HudScale, 
                    SpriteEffects.None, 
                    0
                );
            }

            if (playerInventory.data.ContainsKey(Kind.Heart))
            {
                Option<int> maxHPOption = playerInventory.data[Kind.Heart].maxCountOpt;
                maxHPOption.MatchSome(maxHP => {
                    int currentHP = playerInventory.data[Kind.Heart].Count;
                    RenderHearts(currentHP, maxHP, contentManager, spriteBatch);
                });
            }
        }

        private static void RenderItem(
            Kind kind, 
            ContentManager contentManager, 
            SpriteBatch spriteBatch, 
            int slotNumber
        ) {
            Components.Visibles.Sprite itemSprite = 
                Systems.ToVisible.Items.CreateVisibleComp(kind, contentManager);

            var itemPosition = new Ecs.Components.Position()
            {
                data = HudScale * (itemStartPosition + itemOffset * slotNumber)
            };

            Systems.SpriteRender.Render(itemSprite, itemPosition, spriteBatch);
        }

        private static void RenderHearts(
            int currentHP, 
            int maxHP, 
            ContentManager contentManager, 
            SpriteBatch spriteBatch
        ) {
            bool hasHalfHeart = currentHP % 2 != 0;

            var destinationRectangle = new Rectangle(
                (int)(heartStartPosition.X * HudScale), 
                (int)(heartStartPosition.Y * HudScale), 
                16 * HudScale, 
                16 * HudScale
            );

            for (var i = 1; i <= maxHP / 2; i++)
            {
                if (i * 2 <= currentHP)
                {
                    spriteBatch.Draw(
                        contentManager.Load<Texture2D>("UI/Heart"), 
                        destinationRectangle, fullHeart, 
                        Color.White
                    );
                }
                else if (hasHalfHeart)
                {
                    spriteBatch.Draw(
                        contentManager.Load<Texture2D>("UI/Heart"), 
                        destinationRectangle, halfHeart, 
                        Color.White
                    );
                    hasHalfHeart = false;
                }
                else
                {
                    spriteBatch.Draw(
                        contentManager.Load<Texture2D>("UI/Heart"),
                        destinationRectangle, 
                        emptyHeart, 
                        Color.White
                    );
                }
                destinationRectangle.X += fullHeart.Width * HudScale;
            }
        }
    }
}
