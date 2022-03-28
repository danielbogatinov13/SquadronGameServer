using GameServer.Services.Dtos.Game;

namespace GameServer.Services.Interfaces
{
    public interface IGameService
    {
        int EnterGame(string userId);
        void ExitGame(string userId);
        ActivePlayersDto GetPlayers();
    }
}
