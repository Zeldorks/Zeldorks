using Optional;
using Microsoft.Xna.Framework.Graphics;
using NetGameClient.GameNS;
using Packets = NetGameShared.Net.Protocol.Packets;

using ClientId = System.Int32;

namespace NetGameClient
{
    public class Game
    {
        public World world;
        public Option<ClientId> clientIdOpt;

        public enum State {
            Loading, Ready
        }

        public State state;

        public void SetState(State state)
        {
            this.state = state;
        }

        public Game(GraphicsDevice graphicsDevice)
        {
            SetState(State.Loading);
            world = new World(graphicsDevice);
            clientIdOpt = Option.None<ClientId>();
        }

        public void ProcessWelcome(Packets.Welcome welcome)
        {
            clientIdOpt = welcome.ClientId.Some();
            System.Console.WriteLine(
                "[DEBUG] Client ID: {0}",
                welcome.ClientId
            );
        }

        private void ProcessState()
        {
            if (state != State.Ready && clientIdOpt.HasValue) {
                SetState(State.Ready);
            }
        }

        public void Update()
        {
            ProcessState();

            clientIdOpt.MatchSome(
                clientId => world.UpdatePlayerEntity(clientId)
            );
            world.UpdateCamera();
            world.Update();
        }
    }
}
