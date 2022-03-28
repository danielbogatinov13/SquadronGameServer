using GameServer.Data.Repositories.DomainRepositories.Interfaces;

namespace GameServer.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGameRepository Games { get; }
        IAnswerRepository Answers { get; }
        IRoundRepository Rounds { get; }
        IQuestionRepository Questions { get; }
        int Commit();
    }
}
