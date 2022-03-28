using GameServer.Data.Repositories.DomainRepositories.Interfaces;
using GameServer.Models;

namespace GameServer.Data.Repositories.DomainRepositories
{
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(GameServerDbContext context) : base(context)
        {
        }
    }
}
