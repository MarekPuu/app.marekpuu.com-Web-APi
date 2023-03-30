using AutoMapper;
using Microsoft.EntityFrameworkCore;
using portfolio_api.Contracts;
using portfolio_api.Data;
using portfolio_api.Models.HouseholdUsers;

namespace portfolio_api.Repository
{
    public class HouseholdUserRepository : GenericRepository<HouseholdUser>, IHouseholdUserRepository
    {
        private readonly MarekPuuDbContext _context;
        private readonly IMapper _mapper;

        public HouseholdUserRepository(MarekPuuDbContext context, IMapper mapper) : base(context)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<bool> CanPerformAdminTask(Guid householdId, string userId)
        {
            var userInHousehold = await _context.HouseholdUsers.Where(a => a.AuthServerUserId == userId).FirstOrDefaultAsync();

            if (userInHousehold == null) { return false; }
            if (userInHousehold.RoleId == 2 || userInHousehold.RoleId == 1) { return true; }

            return false;
        }

        public async Task<bool> CanPerformOwnerTask(Guid householdId, string userId)
        {
            var userInHousehold = await _context.HouseholdUsers.Where(a => a.AuthServerUserId == userId).FirstOrDefaultAsync();

            if (userInHousehold == null) { return false; }
            if (userInHousehold.RoleId == 1) { return true; }

            return false;
        }

        public async Task<List<GetUserHouseholdsDto>> GetHouseholdsByUser(string userid)
        {

            var userHouseholds = await _context.HouseholdUsers.Where(x => x.AuthServerUserId == userid)
                .Include(h => h.Household).Include(r => r.Role).Include(a => a.AuthServerUser).Select(h => new
                {
                    householdId = h.HouseholdId,
                    householdName = h.Household.name,
                    RoleName = h.Role.roleName,
                    OwnerName = h.Household.Owner.Email,
                    CreatedAt = h.Household.CreatedAt,
                    OwnerId = h.Household.ownerId
                }).ToListAsync();

            List<GetUserHouseholdsDto> households = new List<GetUserHouseholdsDto>();

            foreach (var household in userHouseholds)
            {
                var newHousehold = new GetUserHouseholdsDto()
                {
                    HouseholdId = household.householdId,
                    HouseholdName = household.householdName,
                    RoleName = household.RoleName,
                    OwnerName = household.OwnerName,
                    CreatedAt = household.CreatedAt,
                    OwnerId = household.OwnerId
                    
                };


                households.Add(newHousehold);
            }

            return households;
        }

        public async Task<HouseholdUser> GetHouseholdUser(Guid householdId, string userId)
        {
            return await _context.HouseholdUsers.Where(h => h.HouseholdId == householdId && h.AuthServerUserId == userId).FirstOrDefaultAsync();
        }

        public async Task<List<GetHouseholdUsersDto>> GetHouseholdUsers(Guid householdId)
        {

            var householdUser = await _context.HouseholdUsers.Where(x => x.HouseholdId == householdId)
                .Include(a => a.AuthServerUser)
                .Include(r => r.Role)
                .Select(h => new
                {
                    RoleName = h.Role.roleName,
                    RoleId = h.Role.roleId,
                    Email = h.AuthServerUser.Email,
                    UserId = h.AuthServerUser.AuthServerUserId,
                }).ToListAsync();

            List<GetHouseholdUsersDto> users = new List<GetHouseholdUsersDto>();

            foreach (var user in householdUser)
            {
                var newUser = new GetHouseholdUsersDto()
                {
                    Email = user.Email,
                    UserId = user.UserId,
                    RoleName = user.RoleName,
                    RoleId = user.RoleId


                };
                users.Add(newUser);
            }


            return users;
        }

        public async Task<bool> IsInHousehold(Guid householdId, string userId)
        {
            var isInhousehold = await _context.HouseholdUsers.Where(h => h.HouseholdId == householdId && h.AuthServerUserId == userId).FirstOrDefaultAsync();

            return isInhousehold != null;

        }

        public async Task<bool> RemoveUserFromHousehold(Guid householdId, string userId)
        {

            var user = await _context.HouseholdUsers.Where(h => h.HouseholdId == householdId && h.AuthServerUserId == userId).FirstOrDefaultAsync();

            if (user.Equals(null)) return false;

            _context.HouseholdUsers.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
