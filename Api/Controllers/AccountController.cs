using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Api.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Shared.Account;
using Shared;
using System;

namespace Api.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private  readonly IAccountRepository _accountRepository;
        private  readonly ILogger<AccountController> _logger;

        public AccountController(IAccountRepository accountRepository, ILogger<AccountController> logger)
        {
            _accountRepository = accountRepository;
            _logger = logger;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var result = await _accountRepository.Login(request);
                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserInfo userInfo)
        {
            try
            {
                var result = await _accountRepository.Register(userInfo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }
    }
}
