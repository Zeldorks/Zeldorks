using System.Collections.Generic;
using System.Collections.Concurrent;
using static NetGameServer.Net.Server;
using LiteNetLib;

using ClientId = System.Int32;
using GameInputs = NetGameShared.Net.Protocol.GameInputs;
using Packets = NetGameShared.Net.Protocol.Packets;

namespace NetGameServer
{
    public class Server
    {
        private Net.Server netServer;
        private Game game;

        public Server()
        {
            netServer = new Net.Server();
            netServer.Start();

            game = new Game(netServer);
        }

        // Number of ticks before processing network packets.
        // This is set to 1, so we process packets on every tick.
        private const int netProcessPeriod = 1;

        // Number of ticks before sending game snapshots.
        // Since sending snapshots is an expensive operation, this is set to 2
        // so that packets are set on every other tick.
        private const int snapPeriod = 2;

        // Whether we should process network packets now
        private bool ShouldProcessNet()
        {
            return Global.tick % netProcessPeriod == 0;
        }

        // Whether we should send network packets now
        private bool ShouldSendSnapshots()
        {
            return Global.tick % snapPeriod == 0;
        }

        private void ProcessConnects(
            ConcurrentQueue<NetPeer> connects
        ) {
            while (!connects.IsEmpty)
            {
                if (connects.TryDequeue(out NetPeer netPeer))
                {
                    if (netServer.ContainsClient(netPeer.Id)) {
                        System.Console.WriteLine("[DEBUG] Got connect from already connected client");
                    } else {
                        netServer.ProcessConnect(netPeer);
                        netServer.SendWelcome(netPeer);
                        game.ProcessConnect(netPeer);
                    }
                }
            }
        }

        private void ProcessDisconnects(
            ConcurrentQueue<NetPeer> disconnects
        ) {
            while (!disconnects.IsEmpty)
            {
                if (disconnects.TryDequeue(out NetPeer netPeer))
                {
                    if (netServer.ContainsClient(netPeer.Id)) {
                        game.ProcessDisconnect(netPeer);
                        netServer.ProcessDisconnect(netPeer);
                    } else {
                        System.Console.WriteLine("[DEBUG] Got disconnect from already disconnected client");
                    }
                }
            }
        }

        private void ProcessGameInputs(
            ConcurrentQueue<(ClientId, Packets.GameInput)> gameInputs
        ) {
            while (!gameInputs.IsEmpty)
            {
                (ClientId, Packets.GameInput) pair;
                if (gameInputs.TryDequeue(out pair))
                {
                    ClientId clientId = pair.Item1;
                    Packets.GameInput gameInput = pair.Item2;
                    if (game.players.ContainsKey(clientId)) {
                        var movement = (GameInputs.Movement)gameInput.Movement;
                        game.players[clientId].ProcessMovement(movement);

                        if (gameInput.NextItemSlotA) {
                            game.players[clientId].LoadItemInSlot(
                                slot: 0,
                                forward: true
                            );
                        }

                        if (gameInput.NextItemSlotB) {
                            game.players[clientId].LoadItemInSlot(
                                slot: 1,
                                forward: true
                            );
                        }

                        if (gameInput.PrevItemSlotA) {
                            game.players[clientId].LoadItemInSlot(
                                slot: 0,
                                forward: false
                            );
                        }

                        if (gameInput.PrevItemSlotB) {
                            game.players[clientId].LoadItemInSlot(
                                slot: 1,
                                forward: false
                            );
                        }

                        HashSet<int> slotsToUse = new HashSet<int>();
                        if (gameInput.UseSlotA) slotsToUse.Add(0);
                        if (gameInput.UseSlotB) slotsToUse.Add(1);
                        game.players[clientId].UseSlots(slotsToUse);
                    } else {
                        System.Console.WriteLine("[DEBUG] Got inputs from non-existent player");
                    }
                }
            }
        }

        private void ProcessGameSnapshotAcks(
            ConcurrentQueue<(ClientId, Packets.GameSnapshotAck)> gameSnapshotAcks
        ) {
            while (!gameSnapshotAcks.IsEmpty)
            {
                (ClientId, Packets.GameSnapshotAck) pair;
                if (gameSnapshotAcks.TryDequeue(out pair))
                {
                    ClientId clientId = pair.Item1;
                    Packets.GameSnapshotAck gameSnapshotAck = pair.Item2;
                    netServer.ProcessGameSnapshotAck(
                        clientId,
                        gameSnapshotAck
                    );
                }
            }
        }

        public void Process(PacketsReceived packets)
        {
            // TODO: should `ConcurrentQueues` be locked?
            ProcessConnects(packets.connects);
            ProcessDisconnects(packets.disconnects);
            ProcessGameInputs(packets.gameInputs);
            ProcessGameSnapshotAcks(packets.gameSnapshotAcks);
        }

        public void Update()
        {
            netServer.Update();

            if (ShouldProcessNet()) {
                Process(netServer.packetsReceived);
            }

            game.Update();

            if (ShouldSendSnapshots()) {
                game.SendSnapshots();
            }
        }

        public void Delete()
        {
            netServer.Stop();
        }
    }
}
