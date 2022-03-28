using GameServer.Data.Repositories.DomainRepositories.Interfaces;
using GameServer.Models;

namespace GameServer.Data.Repositories.DomainRepositories
{
    public class AnswerRepository : GenericRepository<Answer>, IAnswerRepository
    {
        public AnswerRepository(GameServerDbContext context) : base(context)
        {
        }
    }
}
