namespace GameServer.Services.Dtos.Game
{
    public class ActivePlayersDto
    {
        public int NumberOfActivePlayers { get; set; }
        public List<int> GameIds { get; set; }
    }
}
