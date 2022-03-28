using GameServer.Models.Infrastructure;

namespace GameServer.Models
{
    public class Round : CreationAuditedEntity
    {
        public bool IsFinished { get; set; }
        public virtual Question Question { get; set; }
        public virtual ICollection<GameRound> Games { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
