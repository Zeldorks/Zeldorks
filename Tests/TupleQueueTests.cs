using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Concurrent;

using Packets = NetGameShared.Net.Protocol.Packets;
using ClientId = System.Int32;

namespace Tests
{
    [TestClass]
    public class TupleQueueTests
    {
        [TestMethod]
        public void Simple()
        {
            var gameInputs = new ConcurrentQueue<(ClientId, Packets.GameInput)>();

            var g1 = new Packets.GameInput {
                Movement = 1
            };
            var g2 = new Packets.GameInput {
                Movement = 2
            };

            gameInputs.Enqueue((0, g1));
            gameInputs.Enqueue((1, g2));

            while (!gameInputs.IsEmpty)
            {
                (ClientId, Packets.GameInput) pair;
                if (gameInputs.TryDequeue(out pair))
                {
                    ClientId clientId = pair.Item1;
                    Packets.GameInput gameInput = pair.Item2;
                    System.Console.WriteLine("[DEBUG] Client ID: {0}", clientId);
                    System.Console.WriteLine("[DEBUG] Movement: {0}", gameInput.Movement);
                }
            }
        }
    }
}
