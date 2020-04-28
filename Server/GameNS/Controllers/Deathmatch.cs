namespace NetGameServer.GameNS.Controllers
{
    public class Deathmatch : Controller
    {
        public Deathmatch(Game game) : base(game) {
            // Empty
        }

        public override bool CheckWin()
        {
            int scoreLimit = Global.config.ScoreLimit;
            foreach (Player player in game.players.Values) {
                if (player.score >= scoreLimit) return true;
            }

            return false;
        }
    }
}
