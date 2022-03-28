namespace GameServer.Models
{
    public class GameRound
    {
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
        public int RoundId { get; set; }
        public virtual Round Round { get; set; }

    }
}
