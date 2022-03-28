using GameServer.Models.Infrastructure;

namespace GameServer.Models
{
    public class Game : CreationAuditedEntity
    {
        public bool IsFinished { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<GameRound> Rounds { get; set; }
    }
}
