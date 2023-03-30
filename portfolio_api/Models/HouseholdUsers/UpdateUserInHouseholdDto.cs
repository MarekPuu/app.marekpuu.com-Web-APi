using Microsoft.AspNetCore.Mvc;

namespace portfolio_api.Models.HouseholdUsers
{
    public class UpdateUserInHouseholdDto
    {
        [FromBody]
        public string userId { get; set; }
        [FromBody]
        public int roleId { get; set; }
    }
}
