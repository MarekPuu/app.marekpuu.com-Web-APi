using portfolio_api.Data;

namespace portfolio_api.Contracts
{
    public interface IAuthServerUserRepository : IGenericRepository<AuthServerUser>
    {
        Task<AuthServerUser> GetUserByEmail(string email);
        Task<bool> ExistsById(string id);
    }
}
