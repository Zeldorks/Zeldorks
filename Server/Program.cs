using System;
using System.Threading;

namespace NetGameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Global.tick = 0;
            Global.random = new Random();
            Global.config = new Config();

            var server = new Server();
            var tickDuration = TimeSpan.FromMilliseconds(20);

            AppDomain.CurrentDomain.ProcessExit +=
                (sender, eventArgs) => server.Delete();

            while (true)
            {
                var tickStart = DateTime.Now;

                server.Update();

                var elapsed = DateTime.Now - tickStart;
                var sleepDuration = tickDuration - elapsed;
                if (sleepDuration > TimeSpan.Zero) {
                    Thread.Sleep((int)sleepDuration.TotalMilliseconds);
                } else {
                    Console.WriteLine("[WARN] Tick ran for longer than max duration");
                }

                Global.tick++;
            }
        }
    }
}
