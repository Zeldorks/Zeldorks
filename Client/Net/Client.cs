using System;
using System.Collections.Concurrent;
using Optional;
using LiteNetLib;
using LiteNetLib.Utils;

using Ecs = NetGameShared.Ecs;

using NetGameShared.Net.Protocol;
using Packets = NetGameShared.Net.Protocol.Packets;

using Tick = System.UInt32;

namespace NetGameClient.Net
{
    public class Client
    {
        private EventBasedNetListener listener;
        private NetManager client;
        private NetPacketProcessor packetProcessor;
        private Option<NetPeer> server;

        public enum State {
            Offline, Connecting, Online
        }

        // TODO: Does this need a mutex?
        public State state;

        public void SetState(State state)
        {
            this.state = state;
        }

        public class PacketsReceived
        {
            public ConcurrentQueue<Packets.GameSnapshot> gameSnapshots;
            public ConcurrentQueue<Packets.GameSnapshotDelta> gameSnapshotDeltas;
            public ConcurrentQueue<Packets.Welcome> welcomes;

            public PacketsReceived()
            {
                gameSnapshots = new ConcurrentQueue<Packets.GameSnapshot>();
                gameSnapshotDeltas = new ConcurrentQueue<Packets.GameSnapshotDelta>();
                welcomes = new ConcurrentQueue<Packets.Welcome>();
            }
        }

        public PacketsReceived packetsReceived;

        private void StoreGameSnapshot(
            Packets.GameSnapshot gameSnapshotPacket
        ) {
            packetsReceived.gameSnapshots.Enqueue(gameSnapshotPacket);
        }

        private void StoreGameSnapshotDelta(
            Packets.GameSnapshotDelta gameSnapshotPacketDelta
        ) {
            packetsReceived.gameSnapshotDeltas.Enqueue(gameSnapshotPacketDelta);
        }

        private void StoreWelcome(
            Packets.Welcome welcomePacket
        ) {
            packetsReceived.welcomes.Enqueue(welcomePacket);
        }

        public Client()
        {
            SetState(State.Offline);

            packetProcessor = new NetPacketProcessor();

            packetProcessor.RegisterNestedType(
                () => new EcDict<Ecs.Components.Position>()
            );
            packetProcessor.RegisterNestedType(
                () => new EcDict<Ecs.Components.Shapes.Rectangle>()
            );
            packetProcessor.RegisterNestedType(
                () => new EcDict<Ecs.Components.Character>()
            );
            packetProcessor.RegisterNestedType(
                () => new EcDict<Ecs.Components.Projectile>()
            );
            packetProcessor.RegisterNestedType(
                () => new EcDict<Ecs.Components.Item>()
            );
            packetProcessor.RegisterNestedType(
                () => new EcDict<Ecs.Components.Door>()
            );
            packetProcessor.RegisterNestedType(
                () => new EcDict<Ecs.Components.Orientations.Cardinal>()
            );
            packetProcessor.RegisterNestedType(
                () => new EcDict<Ecs.Components.Playable>()
            );
            packetProcessor.RegisterNestedType(
                () => new EcDict<Ecs.Components.Inventory>()
            );

            packetProcessor.RegisterNestedType(
                () => new EcDictDelta<Ecs.Components.Position>()
            );
            packetProcessor.RegisterNestedType(
                () => new EcDictDelta<Ecs.Components.Shapes.Rectangle>()
            );
            packetProcessor.RegisterNestedType(
                () => new EcDictDelta<Ecs.Components.Character>()
            );
            packetProcessor.RegisterNestedType(
                () => new EcDictDelta<Ecs.Components.Projectile>()
            );
            packetProcessor.RegisterNestedType(
                () => new EcDictDelta<Ecs.Components.Item>()
            );
            packetProcessor.RegisterNestedType(
                () => new EcDictDelta<Ecs.Components.Door>()
            );
            packetProcessor.RegisterNestedType(
                () => new EcDictDelta<Ecs.Components.Orientations.Cardinal>()
            );
            packetProcessor.RegisterNestedType(
                () => new EcDictDelta<Ecs.Components.Playable>()
            );
            packetProcessor.RegisterNestedType(
                () => new EcDictDelta<Ecs.Components.Inventory>()
            );

            packetProcessor.SubscribeReusable<Packets.GameSnapshot>(
                StoreGameSnapshot
            );
            packetProcessor.SubscribeReusable<Packets.GameSnapshotDelta>(
                StoreGameSnapshotDelta
            );
            packetProcessor.SubscribeReusable<Packets.Welcome>(
                StoreWelcome
            );

            packetsReceived = new PacketsReceived();
        }

        public void Start()
        {
            listener = new EventBasedNetListener();
            listener.PeerConnectedEvent += (peer) => {
                Console.WriteLine("[DEBUG] Connected peer");
                SetState(State.Online);
            };

            listener.PeerDisconnectedEvent += (peer, disconnetInfo) =>
            {
                Console.WriteLine(
                    "Disconnected: {0}, Reason: {1}",
                    peer.EndPoint,
                    disconnetInfo.Reason
                );
                SetState(State.Offline);
            };

            listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod) => {
                packetProcessor.ReadAllPackets(dataReader, fromPeer);
            };

            client = new NetManager(listener);
            client.Start();
        }

        public void Connect()
        {
            string address = Global.config.ServerAddress;
            int port = Global.config.ServerPort;
            string key = Global.config.ServerConnectionKey;
            NetPeer server = client.Connect(address, port, key);

            Console.WriteLine(
                "Connecting to {0}:{1} with key \"{2}\"",
                address,
                port,
                key
            );

            if (server == null) {
                this.server = Option.None<NetPeer>();
                Console.WriteLine("[DEBUG] Connection request failed");
            } else {
                this.server = server.Some();
                Console.WriteLine("[DEBUG] Sent connection request");
                SetState(State.Connecting);
            }
        }

        public void Disconnect()
        {
            server.Match(
                some: sv => client.DisconnectPeer(sv),
                none: () => Console.WriteLine("[WARN] Not connected")
            );

            SetState(State.Offline);
        }

        public void Send(Packets.GameInput gameInput)
        {
            server.MatchSome(server => {
                packetProcessor.Send<Packets.GameInput>(
                    server,
                    gameInput,
                    DeliveryMethod.ReliableOrdered
                );
            });
        }

        // Send a packet acknowledging that `gameSnapshot` was received
        public void SendSnapshotAck(Tick ackedTick)
        {
            server.MatchSome(server => {
                var ack = new Packets.GameSnapshotAck {
                    AckedTick = ackedTick
                };

                packetProcessor.Send<Packets.GameSnapshotAck>(
                    server,
                    ack,
                    DeliveryMethod.Unreliable
                );
            });
        }

        // Don't send any packets to the server to simulate losing the
        // connection
        public bool DebugDisconnect { get; set; }

        public void Update()
        {
            if (!DebugDisconnect)
                client.PollEvents();
        }

        public void Stop()
        {
            client.Stop();
        }
    }
}
