using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Runtime.CompilerServices;

namespace RedResQ_API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class SessionController : ControllerBase
	{

		[HttpGet("login")]
		public ActionResult<User> Login(string id, string secret)
		{
			Credentials credentials = new Credentials(id, secret);

			try
			{
				var output = SessionService.Login(credentials)!;

				if (output == null)
				{
					return NotFound();
				}
				else
				{
					return Ok(output);
				}
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpPost("register")]
		public ActionResult<User> Register(RawUser user)
		{
			try
			{
				return Ok(SessionService.Register(user));
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}
