using Optional;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using NetGameClient.GameNS.WorldNS;

using Ecs = NetGameShared.Ecs;
using Comps = NetGameShared.Ecs.Components;

using EcsExt = NetGameClient.GameNS.WorldNS.EcsExt;
using MapsExt = NetGameClient.GameNS.WorldNS.MapsExt;

using Packets = NetGameShared.Net.Protocol.Packets;

using ClientId = System.Int32;

namespace NetGameClient.GameNS
{
    public class World
    {
        public EcsExt.Registry ecsRegistry;
        public Map map;
        public Camera camera;
        public Option<Ecs.Entity> playerEntityOpt;

        public World(GraphicsDevice graphicsDevice)
        {
            ecsRegistry = new EcsExt.Registry();

            // Get map from server
            map = new Map("Adventure");

            camera = new Camera();
            camera.SetViewport(graphicsDevice);
        }

        public void UpdatePlayerEntity(ClientId clientId)
        {
            HashSet<Ecs.Entity> entities = ecsRegistry.GetEntities(
                typeof(Comps.Playable)
            );

            if (entities.Count > 0) {
                foreach (Ecs.Entity entity in entities) {
                    var playableComp = ecsRegistry
                        .GetComponentUnsafe<Comps.Playable>(entity);

                    if (playableComp.clientId == clientId) {
                        playerEntityOpt = entity.Some();
                        break;
                    }
                }
            }
        }

        public void UpdateCamera()
        {
            playerEntityOpt.MatchSome(playerEntity =>
                camera.Follow(playerEntity, ecsRegistry)
            );
        }

        public void Update()
        {
            // TODO: Run systems on `ecsRegistry`
            // For now, there are no systems to run
        }

        public void Process(Packets.GameSnapshot gameSnapshot)
        {
            ecsRegistry.Process(gameSnapshot);
        }

        public void Process(Packets.GameSnapshotDelta gameSnapshotDelta)
        {
            ecsRegistry.Process(gameSnapshotDelta);
        }

        public void Draw(
            SpriteBatch spriteBatch,
            ContentManager contentManager
        ) {
            MapsExt.Renderer.Render(map, contentManager, spriteBatch, camera);
            EcsExt.Systems.ToVisible.Characters.Run(ecsRegistry, contentManager);
            EcsExt.Systems.ToVisible.Projectiles.Run(ecsRegistry, contentManager);
            EcsExt.Systems.ToVisible.Items.Run(ecsRegistry, contentManager);
            EcsExt.Systems.ToVisible.Doors.Run(ecsRegistry, contentManager);
            EcsExt.Systems.SpriteRender.Run(ecsRegistry, spriteBatch, camera);
        }
    }
}
