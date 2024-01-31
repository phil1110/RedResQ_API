using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Runtime.CompilerServices;

namespace RedResQ_API.Controllers
{
	[ApiController, Route("[controller]"), Authorize]
	public class AuthController : ControllerBase
	{

		[HttpGet("login")]
		public ActionResult<User> Login(string id, string secret)
		{
            return ActionService.Execute(this, "login", () =>
            {
                Credentials credentials = new Credentials(id, secret);

                var output = AuthService.Login(this, JwtHandler.GetClaims(this), credentials)!;

                if (output == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(output);
                }
            });
		}

		[HttpPost("register")]
		public ActionResult<User> Register(RawUser user)
		{
            return ActionService.Execute(this, "register", () =>
            {
                return Ok(AuthService.Register(JwtHandler.GetClaims(this), user));
            });
		}
	}
}
