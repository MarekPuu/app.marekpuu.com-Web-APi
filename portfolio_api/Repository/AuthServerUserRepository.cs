using Microsoft.EntityFrameworkCore;
using portfolio_api.Contracts;
using portfolio_api.Data;

namespace portfolio_api.Repository
{
    public class AuthServerUserRepository : GenericRepository<AuthServerUser>, IAuthServerUserRepository
    {
        private readonly MarekPuuDbContext _context;

        public AuthServerUserRepository(MarekPuuDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<bool> ExistsById(string id)
        {
            var user = await _context.AuthServerUsers.FirstOrDefaultAsync(u => u.AuthServerUserId == id);
            return user != null;
        }

        public async Task<AuthServerUser> GetUserByEmail(string email)
        {
            return await _context.AuthServerUsers.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
