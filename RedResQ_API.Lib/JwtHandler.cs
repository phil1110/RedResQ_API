using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RedResQ_API.Lib.Models;
using RedResQ_API.Lib.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http.Extensions;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace RedResQ_API.Lib
{
    public class JwtHandler
	{
		public static string CreateToken(ControllerBase controller, User user)
        {
            string ipAddress = GetIpAddress(controller.HttpContext.Request);
            DateTime expiryDate = DateTime.UtcNow.AddDays(30);

			List<Claim> claims = new List<Claim>
			{
				new Claim(ClaimTypes.Version, $"{TokenType.User}"),
                new Claim(ClaimTypes.Expiration, $"{expiryDate.Ticks}"),
                new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
				new Claim(ClaimTypes.Name, user.Username),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(ClaimTypes.Role, $"{user.Role.Id}")
			};

            Task.Run(() => LogToken(ipAddress, expiryDate, user.Id));

            return CreateToken(claims, expiryDate);
		}

		public static string CreateGuestToken(ControllerBase controller)
        {
            string ipAddress = GetIpAddress(controller.HttpContext.Request);
            DateTime expiryDate = DateTime.UtcNow.AddMinutes(15);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Version, $"{TokenType.Guest}"),
                new Claim(ClaimTypes.Expiration, $"{expiryDate.Ticks}"),
                new Claim(ClaimTypes.Role, "1")
            };

            Task.Run(() => LogGuestToken(ipAddress, expiryDate));

            return CreateToken(claims, expiryDate);
        }

		private static string CreateToken(List<Claim> claims, DateTime expiryDate)
		{
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("API_TOKEN_KEY")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: expiryDate,
                    signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

		public static JwtClaims GetClaims(ControllerBase controller)
		{
			JwtClaims claims = null!;
			ClaimsPrincipal user = controller.User;
            TokenType tokenType = Enum.Parse<TokenType>(user.FindFirst(ClaimTypes.Version)?.Value!);

			if (tokenType == TokenType.Guest)
            {
                long ticks = Convert.ToInt64(user.FindFirst(ClaimTypes.Expiration)?.Value);
                long role = Convert.ToInt64(user.FindFirst(ClaimTypes.Role)?.Value);

                claims = new JwtClaims(tokenType, role, new DateTime(ticks));
            }
			else if (tokenType == TokenType.User)
            {
                long ticks = Convert.ToInt64(user.FindFirst(ClaimTypes.Expiration)?.Value);
                long id = Convert.ToInt64(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                string? username = user.FindFirst(ClaimTypes.Name)?.Value;
                string? email = user.FindFirst(ClaimTypes.Email)?.Value;
                long role = Convert.ToInt64(user.FindFirst(ClaimTypes.Role)?.Value);

                claims = new JwtClaims(tokenType, id, username!, email!, role, new DateTime(ticks));
            }

			if(claims != null)
			{
				Task.Run(() => LogAccess(controller, claims));

				return claims!;
			}

			throw new AuthException("Invalid Token Type!");
		}

		private static void LogAccess(ControllerBase controller, JwtClaims claims)
		{
			TokenType tokenType = claims.TokenType;
			DateTime timestamp = DateTime.UtcNow;
            string ipAddress = GetIpAddress(controller.HttpContext.Request);
            long? userId = claims.Id == -1 ? null : claims.Id;
			string url = controller.Request.GetDisplayUrl().Split('?')[0];
			string method = controller.Request.Method;

            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Ac_LogAccess";

            parameters.Add(new SqlParameter { ParameterName = "@tokenType", SqlDbType = SqlDbType.VarChar, Value = tokenType });
            parameters.Add(new SqlParameter { ParameterName = "@timestamp", SqlDbType = SqlDbType.DateTime, Value = timestamp });
            parameters.Add(new SqlParameter { ParameterName = "@ipAddress", SqlDbType = SqlDbType.VarChar, Value = ipAddress });
            parameters.Add(new SqlParameter { ParameterName = "@userId", SqlDbType = SqlDbType.BigInt, Value = userId });
            parameters.Add(new SqlParameter { ParameterName = "@url", SqlDbType = SqlDbType.VarChar, Value = url });
            parameters.Add(new SqlParameter { ParameterName = "@method", SqlDbType = SqlDbType.VarChar, Value = method });

            SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());
        }

        private static void LogToken(string ipAddress, DateTime expiryDate, long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_AcTo_LogToken";

            parameters.Add(new SqlParameter { ParameterName = "@ipAddress", SqlDbType = SqlDbType.VarChar, Value = ipAddress });
            parameters.Add(new SqlParameter { ParameterName = "@timestamp", SqlDbType = SqlDbType.DateTime, Value = expiryDate.AddDays(-30) });
            parameters.Add(new SqlParameter { ParameterName = "@validUntil", SqlDbType = SqlDbType.DateTime, Value = expiryDate });
            parameters.Add(new SqlParameter { ParameterName = "@userId", SqlDbType = SqlDbType.BigInt, Value = id });

            SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());
        }

		private static void LogGuestToken(string ipAddress, DateTime expiryDate)
		{
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Gu_LogGuestToken";

            parameters.Add(new SqlParameter { ParameterName = "@ipAddress", SqlDbType = SqlDbType.VarChar, Value = ipAddress });
            parameters.Add(new SqlParameter { ParameterName = "@timestamp", SqlDbType = SqlDbType.DateTime, Value = expiryDate.AddMinutes(-15) });
            parameters.Add(new SqlParameter { ParameterName = "@validUntil", SqlDbType = SqlDbType.VarChar, Value = expiryDate });

            SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());
        }

        private static string GetIpAddress(HttpRequest request)
        {
            if(request.Headers.TryGetValue("X-Real-IP", out var ip))
            {
                return ip;
            }

            return request.Host.Host;
        }
	}
}