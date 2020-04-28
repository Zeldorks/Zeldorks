namespace NetGameServer.GameNS
{
    // Game logic goes here
    public abstract class Controller
    {
        // Dependencies
        public Game game; // The game that the controller belongs to

        public Controller(Game game)
        {
            this.game = game;
        }

        public abstract bool CheckWin();

        public bool CanSpawn()
        {
            return true;
        }

        public virtual void ResetGame()
        {
            game.world.Reset();
            game.ResetPlayers();
        }

        public void Update()
        {
            if (CheckWin()) {
                System.Console.WriteLine("[DEBUG] Round ended, resetting game");
                ResetGame();
            }
        }
    }
}
