using GameServer.Models;
using GameServer.Models.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GameServer.Data
{
    public class GameServerDbContext : IdentityDbContext<User>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GameServerDbContext(DbContextOptions<GameServerDbContext> options,
            IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        //entities
        public DbSet<Round> Rounds { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Game> Games { get; set; }

        public override int SaveChanges()
        {
            foreach (var history in ChangeTracker.Entries()
                .Where(e => e.Entity is CreationAuditedEntity && (e.State == EntityState.Added || e.State == EntityState.Modified))
                .Select(e => e.Entity as CreationAuditedEntity)
            )
            {
                history.CreatedBy = ((User)_httpContextAccessor.HttpContext.Items["User"]).Id;
                history.CreatedOn = DateTimeOffset.Now;
                if (history.CreatedOn == DateTimeOffset.MinValue)
                {
                    history.CreatedOn = DateTimeOffset.Now;
                }
            }
            var result = base.SaveChanges();
            foreach (var history in ChangeTracker.Entries()
                .Where(e => e.Entity is CreationAuditedEntity)
                .Select(e => e.Entity as CreationAuditedEntity)
            )
            {
                return result;
            }
            return base.SaveChanges();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>()
                .HasOne(a => a.User)
                .WithMany(x => x.Answers)
                .HasForeignKey(x => x.UserId)
                .IsRequired();

            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Round)
                .WithMany(x => x.Answers)
                .HasForeignKey(x => x.RoundId)
                .IsRequired();

            modelBuilder.Entity<Question>()
                .HasOne(a => a.Round)
                .WithOne(x => x.Question)
                .HasForeignKey<Question>(x => x.RoundId)
                .IsRequired();

            modelBuilder.Entity<Game>()
                .HasOne(a => a.User)
                .WithMany(x => x.Games)
                .HasForeignKey(x => x.UserId)
                .IsRequired();

            modelBuilder.Entity<GameRound>()
                .HasKey(t => new { t.GameId, t.RoundId });

            modelBuilder.Entity<GameRound>()
                .HasOne(a => a.Game)
                .WithMany(x => x.Rounds)
                .HasForeignKey(x => x.GameId)
                .IsRequired();

            modelBuilder.Entity<GameRound>()
                .HasOne(a => a.Round)
                .WithMany(x => x.Games)
                .HasForeignKey(x => x.RoundId)
                .IsRequired();


            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
