using portfolio_api.Data;
using portfolio_api.Models.HouseholdUsers;

namespace portfolio_api.Contracts
{
    public interface IHouseholdUserRepository : IGenericRepository<HouseholdUser>
    {
        Task<HouseholdUser> GetHouseholdUser (Guid householdId, string userId);
        Task<List<GetUserHouseholdsDto>> GetHouseholdsByUser(string userid);
        Task<List<GetHouseholdUsersDto>> GetHouseholdUsers(Guid householdId);
        Task<bool> CanPerformAdminTask(Guid householdId,string userId);
        Task<bool> CanPerformOwnerTask(Guid householdId, string userId);
        Task<bool> IsInHousehold(Guid householdId, string userId);
        Task<bool>RemoveUserFromHousehold(Guid householdId, string userId);


    }
}
