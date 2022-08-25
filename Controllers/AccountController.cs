using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace GoogleAuthentication.Controllers
{
    [AllowAnonymous,Route("account")]
    public class AccountController : Controller
    {
        string userId;
        [Route("google-login")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("response") };
            return Challenge(properties,GoogleDefaults.AuthenticationScheme);
        }
        [Route("facebook-login")]
        public IActionResult FacebookLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("response") };
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }
       
        
        [Route("response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result= await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
         
            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claims => new
            {
                claims.Issuer,
                claims.OriginalIssuer,
                claims.Type,
                claims.Value
            });

            var response= result.Principal.Claims.ToList();
            userId = response[0].Value;
            HttpContext.Session.SetString("UserId", userId);
            if (response.Count>1)
            {
                // return Redirect(Request.Headers["Referer"].ToString());
                return RedirectToAction("Index", "Home");
            }else
            {
                return RedirectToAction("Index", "Home");
            }

        }
       
        public IActionResult Logout()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            //Response.Redirect("https://www.google.com/accounts/Logout");
               return RedirectToAction("Index", "Home");
            
            
        }


    }
}
