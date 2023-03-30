using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace portfolio_api.Data
{

    public class Household
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid HouseholdId { get; set; }
        public string name { get; set; }
        public string ownerId { get; set; }
        public AuthServerUser Owner { get; set; }

        public virtual ICollection<HouseholdUser> HouseholdUsers { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
