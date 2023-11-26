using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedResQ_API.Lib;
using RedResQ_API.Lib.Models;
using RedResQ_API.Lib.Services;

namespace RedResQ_API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class JwtTest : ControllerBase
	{
		[HttpGet]
		[Authorize]
		public ActionResult<JwtClaims> Test()
		{
			JwtClaims claims = JwtHandler.GetClaims(this);

			if (claims.Username != "string")
			{
				return Ok(claims);
			}

			return NotFound(claims);
		}
	}
}
