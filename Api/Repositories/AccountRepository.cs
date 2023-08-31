using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Api.Repositories.Interface;
using System.Threading.Tasks;
using Api.Models;
using Shared.Account;
using System;
using Shared;

namespace Api.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ILogger<AccountRepository> _logger;
        private readonly ApplicationDbContext _context;

        public AccountRepository(ApplicationDbContext context, ILogger<AccountRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<UserInfo> Login(LoginRequest loginRequest)
        {
            try
            {
                return  await _context.UserInfo.FirstOrDefaultAsync(s => s.Email == loginRequest.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return default(UserInfo);
            }
        }
        public async Task<UserInfo> Register(UserInfo userInfo)
        {
            try
            {
                var user = await _context.UserInfo.FirstOrDefaultAsync(s => s.Email == userInfo.Email);
                if(user == null) {
                    await _context.UserInfo.AddAsync(userInfo);
                    await _context.SaveChangesAsync();

                }

                return await _context.UserInfo.FirstOrDefaultAsync(s => s.Email == userInfo.Email);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return default(UserInfo);
            }
        }

    }
}
