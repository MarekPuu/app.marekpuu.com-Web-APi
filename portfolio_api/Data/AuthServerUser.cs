using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace portfolio_api.Data
{
    public class AuthServerUser
    {
        [Key]
        public string AuthServerUserId { get; set; }
        public string Email { get; set; }

        public virtual ICollection<HouseholdUser> HouseholdUsers { get; set; }

        public DateTime Joined { get; set; } = DateTime.UtcNow;
    }
}
