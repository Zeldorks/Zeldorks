using System;

namespace NetGameClient
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Client())
                game.Run();
        }
    }
}
