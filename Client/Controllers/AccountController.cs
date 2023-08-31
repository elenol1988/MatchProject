using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Client.Client.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Claims;
using JWTAuthentication;
using Shared.JWToken;
using Shared.Account;
using NToastNotify;
using Shared;
using System;

namespace Client.Controllers
{
    public class AccountController : Controller
    {
        private static  IAccountClient _accountClient;
        private static  ILogger<AccountController> _logger;
        private readonly IToastNotification _toastNotification;
        private static  IJWTokenManager _jWTokenManager;
 
        public AccountController( IAccountClient accountClient,ILogger<AccountController> logger,
                                  IToastNotification toastNotification,
                                  IJWTokenManager jWTokenManager) 
        {
            _accountClient = accountClient;
            _logger = logger;
            _toastNotification = toastNotification;
            _jWTokenManager = jWTokenManager;

        }

        [HttpGet]
        public async Task<IActionResult> Login(LoginRequest loginResonse =null)
        {
            return View(loginResonse);
        }
        [HttpPost]
        [ActionName("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginPost(LoginRequest loginRequest)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var tokenRequest = new TokenGenerationRequest()
                    {
                        Email = loginRequest.Email,
                        JwtAudience = Startup.Audience,
                        JWTSubject = Startup.Subject,
                        JWTIssuer = Startup.Issuer,
                        JWTKey = Startup.Key

                    };
                    var jwToken = _jWTokenManager.GenerateToken(tokenRequest);
                    var userInfo = await _accountClient.Login(loginRequest, jwToken.Token);
                    if (userInfo != null)
                    {
                        if(loginRequest.Password != userInfo.Password)
                        {
                            _toastNotification.AddInfoToastMessage("Wrong password");
                            return View(loginRequest);
                        }
                       
                        IdentitySignin(userInfo, jwToken);
                        return RedirectToAction("Index", "Matches");
                    }
                    _toastNotification.AddInfoToastMessage("Does not exist this user");
                    return View();
                }
                _toastNotification.AddErrorToastMessage("Something went wrong. Please try Again");
                return View(loginRequest);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                _toastNotification.AddErrorToastMessage("Something went wrong. Please try Again");
                return View(loginRequest);

            }
            
        }
        public async void IdentitySignin(UserInfo userInfo, JWTokenResult token , bool isPersistent = false)
        {
            try
            {
                ClaimsIdentity identity =  new ClaimsIdentity(new[] 
                {
                    new Claim(ClaimTypes.NameIdentifier, userInfo.Id.ToString()),
                    new Claim(ClaimTypes.Name, userInfo.UserName),
                    new Claim(ClaimTypes.Email, userInfo.Email),
                    new Claim("access_token",token.Token)

                }, 
                CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);
                var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        IsPersistent = isPersistent,
                        ExpiresUtc = token.Expiry
                    });
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex.Message);
               
            }
        }
        public async Task Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.Clear();
        }

        [HttpGet]
        public async Task<IActionResult> Register(UserInfo userInfo)
        {
            return View(userInfo);
        }

        [HttpPost]
        [ActionName("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterPost(UserInfo userInfo)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var tokenRequest = new TokenGenerationRequest()
                    {
                        Email = userInfo.Email,
                        JwtAudience = Startup.Audience,
                        JWTSubject = Startup.Subject,
                        JWTIssuer = Startup.Issuer,
                        JWTKey = Startup.Key

                    };
                    var jwToken = _jWTokenManager.GenerateToken(tokenRequest);
                    userInfo.CreatedDate = DateTime.Now;
                    var createdUser = await _accountClient.Register(userInfo, jwToken.Token);
                    if (createdUser != null)
                    {
                        var model = new LoginRequest
                        {
                            Email = userInfo.Email,
                            Password = userInfo.Password
                        };
                        return RedirectToAction("Login", "Account", model);
                    }
                }
                return View(userInfo);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                _toastNotification.AddErrorToastMessage("Something went wrong. Please try Again");
                return View(userInfo);
            }
          
        }
    }
}
