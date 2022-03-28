namespace GameServer.Models.Infrastructure
{
    public class CreationAuditedEntity : Entity
    {
        public DateTimeOffset CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }
}
