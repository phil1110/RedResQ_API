using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedResQ_API.Lib;
using RedResQ_API.Lib.Models;
using RedResQ_API.Lib.Services;

namespace RedResQ_API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class NewsController : ControllerBase
	{
		public NewsController()
		{
		}

		[HttpGet]
		[Authorize]
		public ActionResult<Article> First()
		{
			return Ok(Person?.);
		}
	}
}
