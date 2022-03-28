using GameServer.Services.Dtos.Round;

namespace GameServer.Services.Interfaces
{
    public interface IRoundService
    {
        RoundDto CreateRound();
        RoundDto AnswerRound(RoundDto roundDto, string userId);
        RoundDto GetActiveRound();
    }
}
