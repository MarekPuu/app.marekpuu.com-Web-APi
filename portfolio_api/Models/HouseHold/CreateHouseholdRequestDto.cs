using System.ComponentModel.DataAnnotations;

namespace portfolio_api.Models.HouseHold
{
    public class CreateHouseholdRequestDto
    {
        [StringLength(30, ErrorMessage = "Validation failed: stringlength not between 1 and 30", MinimumLength = 1)]
        public string name { get; set; }
    }
}
