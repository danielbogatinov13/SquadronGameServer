using Microsoft.AspNetCore.Identity;

namespace GameServer.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Game> Games { get; set; }
    }
}
