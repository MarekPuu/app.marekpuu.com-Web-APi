using System.ComponentModel.DataAnnotations;

namespace portfolio_api.Models.HouseholdUsers
{
    public class GetUserHouseholdsDto
    {
        [Required]
        public Guid HouseholdId { get; set; }
        [Required]
        public string HouseholdName { get; set; }
        [Required]
        public string RoleName { get; set; }
        [Required]
        public string OwnerName { get; set; }
        [Required]
        public string OwnerId { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

    }
}
