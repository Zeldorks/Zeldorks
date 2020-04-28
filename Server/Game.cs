using System.Collections.Generic;
using LiteNetLib;
using Optional;

using NetGameServer.GameNS;
using NetClient = NetGameServer.Net.Client;
using ClientId = System.Int32;
using Packets = NetGameShared.Net.Protocol.Packets;

namespace NetGameServer
{
    public class Game
    {
        public World world;
        public Controller controller;
        public Dictionary<ClientId, Player> players;
        public GameSnapshotCache gameSnapshotCache;

        // Dependencies
        private Net.Server netServer;

        public Game(Net.Server netServer)
        {
            this.netServer = netServer;

            world = new World(this);
            controller = new GameNS.Controllers.Adventure(this);
            players = new Dictionary<ClientId, Player>();
            gameSnapshotCache = new GameSnapshotCache(world);
        }

        // Assume that `netPeer` is not already connected
        public void ProcessConnect(NetPeer netPeer)
        {
            players.Add(netPeer.Id, new Player(netPeer.Id, world, controller));
            System.Console.WriteLine("[DEBUG] Added new player");
        }

        // Assume that `netPeer` is not already disconnected
        public void ProcessDisconnect(NetPeer netPeer)
        {
            players[netPeer.Id].ProcessDisconnect();
            players.Remove(netPeer.Id);
            System.Console.WriteLine("[DEBUG] Removed player");
        }

        public void Update()
        {
            foreach (Player player in players.Values) {
                player.Update();
            }
            world.Update();
            controller.Update();
        }

        public void SendSnapshots()
        {
            foreach (NetClient client in netServer.clients.Values) {
                if (client.ShouldSendSnapshot) {
                    client.ackedTickOpt.Match(
                        some: ackedTick => {
                            Option<Packets.GameSnapshot> baseSnapOpt =
                                gameSnapshotCache.GetCached(ackedTick);

                            baseSnapOpt.Match(
                                some: baseSnap => {
                                    // Create delta
                                    var currentSnap = gameSnapshotCache.GetCurrent();
                                    var deltaSnap = new Packets.GameSnapshotDelta(
                                        currentSnap,
                                        baseSnap
                                    );

                                    netServer.Send(client.Id, deltaSnap);
                                },
                                none: () => {
                                    // Base snapshot has been discarded, so we
                                    // can't use delta compression. Therefore we
                                    // have to send a full snapshot.
                                    client.SetSnapRateState(NetClient.SnapRateState.Recovery);
                                    var currentSnap = gameSnapshotCache.GetCurrent();
                                    netServer.Send(client.Id, currentSnap);
                                }
                            );
                        },
                        none: () => {
                            // Client hasn't acked a single snapshot yet, so we
                            // have to send them a full snapshot.
                            var currentSnap = gameSnapshotCache.GetCurrent();
                            netServer.Send(client.Id, currentSnap);
                        }
                    );
                }
            }
        }

        public void ResetPlayers()
        {
            foreach (Player player in players.Values) {
                player.score = 0;
            }
        }
    }
}
