using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using portfolio_api.Contracts;
using portfolio_api.Data;

namespace portfolio_api.Repository
{
    public class HouseholdRepository : GenericRepository<Household>, IHouseholdRepository
    {
        private readonly MarekPuuDbContext _context;

        public HouseholdRepository(MarekPuuDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<bool> CanRequestHousehold(Guid householdId, string userId)
        {
            var isInHousehold = await _context.HouseholdUsers.FirstOrDefaultAsync(h => h.HouseholdId == householdId && h.AuthServerUserId == userId);

            return isInHousehold != null;
        }

        public async Task<bool> DeleteAsyncByGuid(Guid householdId)
        {
            var household = await _context.Households.FirstOrDefaultAsync(h => h.HouseholdId == householdId);
            if (household == null)
            {
                return false;
            }

            _context.Households.Remove(household);
            await _context.SaveChangesAsync();

            return true;
        }



        public async Task<bool> DuplicatedHouseholdNameToUser(string name, string owner)
        {
            var duplicatedHouseholdName = await _context.Households.FirstOrDefaultAsync(h => h.name == name && h.ownerId == owner);

            return duplicatedHouseholdName != null;
        }

        public async Task<Household> GetAsyncByGuid(Guid householdId)
        {
            var household = await _context.Households.FirstOrDefaultAsync(h => h.HouseholdId == householdId);
            return household;
        }
    }
}
