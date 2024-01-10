using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using RedResQ_API.Lib.Exceptions;
using RedResQ_API.Lib.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Services
{
	public static class AuthService
	{
		public static string Register(JwtClaims claims, RawUser user)
		{
            if (PermissionService.IsPermitted("register", claims.Role))
            {
                if (user != null)
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    string storedProcedure = "SP_Se_Register";
                    user.Hash = HashPassword(user.Hash);

                    parameters.Add(new SqlParameter { ParameterName = "@username", SqlDbType = SqlDbType.VarChar, Value = user.Username });
                    parameters.Add(new SqlParameter { ParameterName = "@firstname", SqlDbType = SqlDbType.VarChar, Value = user.FirstName });
                    parameters.Add(new SqlParameter { ParameterName = "@lastname", SqlDbType = SqlDbType.VarChar, Value = user.LastName });
                    parameters.Add(new SqlParameter { ParameterName = "@email", SqlDbType = SqlDbType.VarChar, Value = user.Email });
                    parameters.Add(new SqlParameter { ParameterName = "@birthdate", SqlDbType = SqlDbType.DateTime, Value = user.Birthdate });
                    parameters.Add(new SqlParameter { ParameterName = "@hash", SqlDbType = SqlDbType.VarChar, Value = user.Hash });
                    parameters.Add(new SqlParameter { ParameterName = "@gender", SqlDbType = SqlDbType.BigInt, Value = user.Gender });
                    parameters.Add(new SqlParameter { ParameterName = "@languageId", SqlDbType = SqlDbType.BigInt, Value = user.Language });
                    parameters.Add(new SqlParameter { ParameterName = "@locationId", SqlDbType = SqlDbType.BigInt, Value = user.Location });
                    parameters.Add(new SqlParameter { ParameterName = "@roleId", SqlDbType = SqlDbType.BigInt, Value = 2 });

                    SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

                    return "Registered Successfully!";
                }
            }
            
			throw new UnauthorizedAccessException("Person object was null!");
		}

		public static string Login(ControllerBase controller, JwtClaims claims, Credentials credentials)
		{
            if (PermissionService.IsPermitted("login", claims.Role))
            {
                if (credentials != null)
                {
                    User user;

                    if (credentials.Identifier.Contains("@"))
                    {
                        user = LoginEmail(credentials);
                    }
                    else
                    {
                        user = LoginUsername(credentials);
                    }

                    return JwtHandler.CreateToken(controller, user);
                }
            }

			throw new UnauthorizedAccessException("Credentials object was null!");
		}

        internal static User LoginEmail(Credentials credentials)
		{
			List<SqlParameter> parameters = new List<SqlParameter>();
			string storedProcedure = "SP_Se_LoginEmail";

			parameters.Add(new SqlParameter { ParameterName = "@email", SqlDbType = SqlDbType.VarChar, Value = credentials.Identifier });

			DataTable userTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

			if(userTable.Rows.Count == 1)
			{
				User output = User.ConvertToPerson(userTable.Rows[0]);

				if (BCrypt.Net.BCrypt.Verify(credentials.Secret, GetHash(output)))
				{
					return output;
				}
				else
				{
					throw new ForbidException();
				}
			}
			else
			{
				throw new Exception("User was not found!");
			}
		}

		internal static User LoginUsername(Credentials credentials)
		{
			List<SqlParameter> parameters = new List<SqlParameter>();
			string storedProcedure = "SP_Se_LoginUsername";

			parameters.Add(new SqlParameter { ParameterName = "@username", SqlDbType = SqlDbType.VarChar, Value = credentials.Identifier });

			DataTable userTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

			if (userTable.Rows.Count == 1)
			{
				User output = User.ConvertToPerson(userTable.Rows[0]);

				if (BCrypt.Net.BCrypt.Verify(credentials.Secret, GetHash(output)))
				{
					return output;
				}
				else
				{
					throw new UnauthorizedAccessException();
				}
			}
			else
			{
				throw new Exception("User was not found!");
			}
		}

		internal static string HashPassword(string hash)
		{
			return BCrypt.Net.BCrypt.HashPassword(hash);
		}

		private static string GetHash(User user)
		{
			List<SqlParameter> parameters = new List<SqlParameter>();
			string storedProcedure = "SP_Pe_GetHash";

			parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = user.Id });

			DataTable hash = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

			return Convert.ToString(hash!.Rows[0]!.ItemArray[0]!)!;
		}
	}
}
