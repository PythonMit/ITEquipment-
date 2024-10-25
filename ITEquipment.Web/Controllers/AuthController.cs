using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using RestSharp;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;

namespace ITEquipment.Web.Controllers
{
    [Authorize]
    public class AuthController : Controller
    {

        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Authenticate()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {

                    var redirectUrl = _configuration.GetSection("WebHostURL").Value + "/Home/Index";
                    var properties = new AuthenticationProperties
                    {
                        RedirectUri = redirectUrl
                    };

                    properties.Parameters["prompt"] = "select_account";
                    return Challenge(properties, OpenIdConnectDefaults.AuthenticationScheme);

                }
                return RedirectToAction("Unauthorized", "Home");
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IActionResult Login()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Home");
                }
                return View();
            }
            catch (Exception)
            {

                throw;
            }

            
        }
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignOutAsync("OpenIdConnect");
                var callbackUrl = Url.Action("Login", "Auth", values: null, protocol: Request.Scheme);
                //var callbackUrl = Url.Action("Privacy", "Home", values: null, protocol: Request.Scheme);
                var properties = new AuthenticationProperties
                {
                    RedirectUri = callbackUrl
                };
                HttpContext.Session.Clear();
                properties.Parameters["prompt"] = "none";
                return SignOut(
                    properties,
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    OpenIdConnectDefaults.AuthenticationScheme);
            }
            catch (Exception)
            {

                throw;
            }
           

        }

    }
}
