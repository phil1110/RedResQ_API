using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RedResQ_API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class JwtTest : ControllerBase
	{
		[HttpGet]
		[Authorize]
		public ActionResult<JwtClaims> Test(string temp)
		{
			return ActionService.Execute(this, () =>
			{
				JwtClaims claims = JwtHandler.GetClaims(this);

				if (claims.Username != "string")
				{
					return Ok(claims);
				}

				return NotFound(claims);
			});
		}
	}
}
