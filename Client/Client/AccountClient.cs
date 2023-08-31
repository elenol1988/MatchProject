using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Client.Client.Interfaces;
using System.Threading.Tasks;
using ClientProvider;
using Shared.Account;
using Shared;
using System;

namespace Client.Client
{
    public class AccountClient : IAccountClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<IAccountClient> _logger;
        private readonly IClientProvider _clientProvider;
        public AccountClient(IHttpContextAccessor httpContextAccessor, ILogger<IAccountClient> logger,IClientProvider clientProvider) 
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _clientProvider = clientProvider;
           
        }
        public async Task<UserInfo> Login(LoginRequest loginRequest, string token)
        {
            try
            {
                string url = String.Format(Startup.ApiUrl + "/account/login/");
                return await _clientProvider.PostAsync<UserInfo, LoginRequest>(url,token,  loginRequest);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return default(UserInfo);
            }
        }
        public async Task<UserInfo> Register(UserInfo userInfo, string token)
        {
            try
            {
                string url = String.Format(Startup.ApiUrl + "/account/register/");
                return await _clientProvider.PostAsync<UserInfo, UserInfo>(url,token, userInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return default(UserInfo);
            }
        }
    }
}
