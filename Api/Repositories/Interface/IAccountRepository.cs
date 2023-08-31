using Shared;
using Shared.Account;
using Shared.JWToken;
using System.Threading.Tasks;

namespace Api.Repositories.Interface
{
    public interface IAccountRepository
    {
        Task<UserInfo> Login(LoginRequest loginRequest);
        Task<UserInfo> Register(UserInfo userInfo);
    }
}
