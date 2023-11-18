﻿using Microsoft.AspNetCore.Authorization;
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
			JwtClaims claims = GetClaims();

			if(claims.Username != "string")
			{
				return Ok("Hello!" + claims);
			}

			return NotFound(claims);
		}

		private JwtClaims GetClaims()
		{
			string? id = User.FindFirstValue(ClaimTypes.NameIdentifier);
			string? username = User.FindFirstValue(ClaimTypes.Name);
			string? email = User.FindFirstValue(ClaimTypes.Email);
			int role = Convert.ToInt32(User.FindFirstValue(ClaimTypes.Role));

			return new JwtClaims(id!, username!, email!, role); ;
		}
	}
}
