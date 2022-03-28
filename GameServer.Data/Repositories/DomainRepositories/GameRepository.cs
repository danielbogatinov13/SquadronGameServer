using GameServer.Data.Repositories.DomainRepositories.Interfaces;
using GameServer.Models;

namespace GameServer.Data.Repositories.DomainRepositories
{
    public class GameRepository : GenericRepository<Game>, IGameRepository
    {
        public GameRepository(GameServerDbContext context) : base(context)
        {
        }
    }
}
