using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using System.Linq;

namespace Client.Controllers
{
    public class BaseController : Controller
    {
       

        public int CurrentUserId
        {
            get
            {
                var identity = User.Identity as ClaimsIdentity;
                Claim identityClaim = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (identityClaim != null && !String.IsNullOrEmpty(identityClaim.Value)) return Convert.ToInt32(identityClaim.Value);

                return 0;
            }
        }

        protected string GetToken
        {
            get
            {
                var identity = User.Identity as ClaimsIdentity;
                Claim identityClaim = identity.Claims.FirstOrDefault(c => c.Type == "access_token");
                if (identityClaim != null && !String.IsNullOrEmpty(identityClaim.Value)) return identityClaim.Value;

                return String.Empty;
            }

        }

      

        protected string CurrentUserName
        {
            get
            {
                var identity = User.Identity as ClaimsIdentity;
                Claim identityClaim = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                if (identityClaim != null && !String.IsNullOrEmpty(identityClaim.Value)) return identityClaim.Value;

                return null;
            }
        }
      
    }
}
