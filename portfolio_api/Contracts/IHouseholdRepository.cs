using portfolio_api.Data;

namespace portfolio_api.Contracts
{
    public interface IHouseholdRepository : IGenericRepository<Household>
    {
        Task<bool> DuplicatedHouseholdNameToUser(string name, string owner);
        Task<bool> CanRequestHousehold(Guid householdId, string userId);
        Task<Household> GetAsyncByGuid(Guid householdId);
        Task<bool> DeleteAsyncByGuid(Guid householdId);

    }
}
