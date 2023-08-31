using System.Threading.Tasks;
using Shared.Account;
using Shared;

namespace Client.Client.Interfaces
{
    public interface IAccountClient
    {
        Task<UserInfo> Login(LoginRequest loginResonse, string token);
        Task<UserInfo> Register(UserInfo userInfo, string token);
    }
}
