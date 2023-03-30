using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace portfolio_api.Data
{
    public class HouseholdUser
    {
        public string AuthServerUserId { get; set; }
        public AuthServerUser AuthServerUser { get; set; }

        public Guid HouseholdId { get; set; }
        public Household Household { get; set; }


        [ForeignKey(nameof(RoleId))]
        public int RoleId { get; set; } = 3;
        public Role Role { get; set; }

        public DateTime MemberSince { get; set; } = DateTime.UtcNow;

    }
}