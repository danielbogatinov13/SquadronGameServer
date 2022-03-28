using GameServer.Data.Repositories.DomainRepositories.Interfaces;
using GameServer.Models;

namespace GameServer.Data.Repositories.DomainRepositories
{
    public class RoundRepository : GenericRepository<Round>, IRoundRepository
    {
        public RoundRepository(GameServerDbContext context) : base(context)
        {
        }
    }
}
