using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoSecurity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public IActionResult Login()
        {
            Response.Cookies.Append("RefreshToken", "valeur du refresh token", new CookieOptions
            {
                //SameSite = SameSiteMode.Strict // CRSF Ne marche pas avec Angular
                SameSite = SameSiteMode.None,
                HttpOnly = true
            });

            return Ok(new
            {
                accessToken = "ey.....",
                // refreshToken = "autre chose"
            });
        }

        [HttpGet]
        public IActionResult RefreshToken() 
        {
            string? cookieValue;
            Request.Cookies.TryGetValue("RefreshToken", out cookieValue);
            // if cookieValue is null || cookieValue is invalid
            // return Forbid()
            return Ok(new {
                AccessToken = "ey...." 
            });
        }
    }
}
