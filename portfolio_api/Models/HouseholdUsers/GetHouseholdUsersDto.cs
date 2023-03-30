using System.ComponentModel.DataAnnotations;

namespace portfolio_api.Models.HouseholdUsers
{
    public class GetHouseholdUsersDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string RoleName { get; set; }
        [Required]
        public int RoleId { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
