using GameServer.Models.Infrastructure;

namespace GameServer.Models
{
    public class Answer : CreationAuditedEntity
    {
        public bool IsCorrect { get; set; }
        public int RoundId { get; set; }
        public virtual Round Round { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}