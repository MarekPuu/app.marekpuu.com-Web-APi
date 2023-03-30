using Microsoft.AspNetCore.Mvc;

namespace portfolio_api.Models.HouseholdUsers
{
    public class AddUserToHouseholdDto
    {
        [FromBody]
        public string Email { get; set; }
        
    }
}
