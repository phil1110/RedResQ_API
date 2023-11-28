using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RedResQ_API.Lib.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib
{
	public class JwtHandler
	{
		public static string CreateToken(IUser user)
		{
			List<Claim> claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, ""),
				new Claim(ClaimTypes.Name, user.Username),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(ClaimTypes.Role, $"{user.Role.Id}")
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("API_TOKEN_KEY")!));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var token = new JwtSecurityToken(
					claims: claims,
					expires: DateTime.UtcNow.AddDays(30),
					signingCredentials: creds
				);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public static JwtClaims GetClaims(ControllerBase controller)
		{
			ClaimsPrincipal user = controller.User;

			string? id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			string? username = user.FindFirst(ClaimTypes.Name)?.Value;
			string? email = user.FindFirst(ClaimTypes.Email)?.Value;
			long role = Convert.ToInt64(user.FindFirst(ClaimTypes.Role)?.Value);

			return new JwtClaims(id!, username!, email!, role); ;
		}
	}
}
