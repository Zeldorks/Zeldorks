using System;
using System.Collections.Concurrent;
using Optional;
using LiteNetLib;
using LiteNetLib.Utils;

using Ecs = NetGameShared.Ecs;

using NetGameShared.Net.Protocol;
using Packets = NetGameShared.Net.Protocol.Packets;
using ClientId = System.Int32;

namespace NetGameServer.Net
{
    public class Server
    {
        private EventBasedNetListener listener;
        private NetManager server;
        private NetPacketProcessor packetProcessor;

        public ConcurrentDictionary<ClientId, Client> clients;

        public class PacketsReceived
        {
            public ConcurrentQueue<(ClientId, Packets.GameInput)> gameInputs;
            public ConcurrentQueue<(ClientId, Packets.GameSnapshotAck)> gameSnapshotAcks;

            // Newly connected clients
            public ConcurrentQueue<NetPeer> connects;

            // Newly disconnected clients
            public ConcurrentQueue<NetPeer> disconnects;

            public PacketsReceived()
            {
                gameInputs = new ConcurrentQueue<(ClientId, Packets.GameInput)>();
                gameSnapshotAcks = new ConcurrentQueue<(ClientId, Packets.GameSnapshotAck)>();
                connects = new ConcurrentQueue<NetPeer>();
                disconnects = new ConcurrentQueue<NetPeer>();
            }
        }

        public PacketsReceived packetsReceived;

        public bool ContainsClient(ClientId clientId)
        {
            return clients.ContainsKey(clientId);
        }

        public Option<Client> GetClient(ClientId clientId)
        {
            if (clients.ContainsKey(clientId)) {
                return clients[clientId].Some();
            } else {
                return Option.None<Client>();
            }
        }

        public void StoreGameInput(
            Packets.GameInput gameInput,
            NetPeer peer
        ) {
            packetsReceived.gameInputs.Enqueue((peer.Id, gameInput.Clone()));
        }

        public void StoreGameSnapshotAck(
            Packets.GameSnapshotAck gameSnapshotAck,
            NetPeer peer
        ) {
            packetsReceived.gameSnapshotAcks.Enqueue(
                (peer.Id, gameSnapshotAck.Clone())
            );
        }

        public Server()
        {
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

            packetProcessor.SubscribeReusable<Packets.GameInput, NetPeer>(
                StoreGameInput
            );
            packetProcessor.SubscribeReusable<Packets.GameSnapshotAck, NetPeer>(
                StoreGameSnapshotAck
            );

            clients = new ConcurrentDictionary<ClientId, Client>();
            packetsReceived = new PacketsReceived();
        }

        public void Start()
        {
            listener = new EventBasedNetListener();
            server = new NetManager(listener);

            listener.ConnectionRequestEvent += request =>
            {
                if (server.PeersCount < 64 /* max connections */) {
                    string key = Global.config.ConnectionKey;
                    request.AcceptIfKey(key);
                } else {
                    request.Reject();
                }
            };

            listener.PeerConnectedEvent += peer =>
            {
                Console.WriteLine("Connected: {0}", peer.EndPoint);
                packetsReceived.connects.Enqueue(peer);
            };

            listener.PeerDisconnectedEvent += (peer, disconnetInfo) =>
            {
                Console.WriteLine(
                    "Disconnected: {0}, Reason: {1}",
                    peer.EndPoint,
                    disconnetInfo.Reason
                );

                packetsReceived.disconnects.Enqueue(peer);
            };

            listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod) =>
            {
                packetProcessor.ReadAllPackets(dataReader, fromPeer);
            };

            int port = Global.config.Port;
            server.Start(port);
            Console.WriteLine("Started server at port {0}", port);
        }

        public void Send(
            ClientId clientId,
            Packets.GameSnapshot gameSnapshot
        ) {
            // TODO: Use `DeliveryMethod.Unreliable`.
            // Currently it's `ReliableUnordered` because LiteNetLib provides
            // automatic fragmentation for large packets if it uses a reliable
            // delivery method.
            packetProcessor.Send(
                clients[clientId].netPeer,
                gameSnapshot,
                DeliveryMethod.ReliableUnordered
            );
        }

        public void Send(
            ClientId clientId,
            Packets.GameSnapshotDelta gameSnapshotDelta
        ) {
            // TODO: See TODO comment above about delivery method
            packetProcessor.Send(
                clients[clientId].netPeer,
                gameSnapshotDelta,
                DeliveryMethod.ReliableUnordered
            );
        }

        public void SendWelcome(NetPeer netPeer)
        {
            packetProcessor.Send(
                netPeer,
                new Packets.Welcome() {
                    ClientId = netPeer.Id
                },
                DeliveryMethod.ReliableUnordered
            );
        }

        public void Update()
        {
            server.PollEvents();
        }

        // Assume that `netPeer` is not already connected
        public void ProcessConnect(NetPeer netPeer)
        {
            clients.TryAdd(netPeer.Id, new Net.Client(netPeer));
        }

        // Assume that `netPeer` is not already disconnected
        public void ProcessDisconnect(NetPeer netPeer)
        {
            clients.TryRemove(netPeer.Id, out _);
        }

        public void ProcessGameSnapshotAck(
            ClientId clientId,
            Packets.GameSnapshotAck gameSnapshotAck
        ) {
            Option<Client> clientOpt = GetClient(clientId);
            clientOpt.MatchSome(client => {
                client.ackedTickOpt = gameSnapshotAck.AckedTick.Some();
                client.SetSnapRateState(Client.SnapRateState.Full);
            });
        }

        public void Stop()
        {
            server.Stop();
        }
    }
}
