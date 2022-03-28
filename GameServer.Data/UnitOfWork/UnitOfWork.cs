using GameServer.Data.Repositories.DomainRepositories;
using GameServer.Data.Repositories.DomainRepositories.Interfaces;

namespace GameServer.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GameServerDbContext _context;
        public UnitOfWork(GameServerDbContext context)
        {
            _context = context;
            Games = new GameRepository(_context);
            Answers = new AnswerRepository(_context);
            Rounds = new RoundRepository(_context);
            Questions = new QuestionRepository(_context);
        }
        public IGameRepository Games { get; private set; }
        public IAnswerRepository Answers { get; private set; }
        public IRoundRepository Rounds { get; private set; }
        public IQuestionRepository Questions { get; private set; }
        public int Commit()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
