﻿using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using RedResQ_API.Lib.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Services
{
	public static class SessionService
	{
		public static string Register(Person person)
		{
			if (person != null)
			{
				List<SqlParameter> parameters = new List<SqlParameter>();
				string storedProcedure = "SP_Se_Register";
				person.Hash = HashPassword(person);

				parameters.Add(new SqlParameter { ParameterName = "@username", SqlDbType = SqlDbType.VarChar, Value = person.Username } );
				parameters.Add(new SqlParameter { ParameterName = "@firstname", SqlDbType = SqlDbType.VarChar, Value = person.FirstName });
				parameters.Add(new SqlParameter { ParameterName = "@lastname", SqlDbType = SqlDbType.VarChar, Value = person.LastName });
				parameters.Add(new SqlParameter { ParameterName = "@email", SqlDbType = SqlDbType.VarChar, Value = person.Email });
				parameters.Add(new SqlParameter { ParameterName = "@birthdate", SqlDbType = SqlDbType.DateTime, Value = person.Birthdate });
				parameters.Add(new SqlParameter { ParameterName = "@hash", SqlDbType = SqlDbType.VarChar, Value = person.Hash });
				parameters.Add(new SqlParameter { ParameterName = "@sex", SqlDbType = SqlDbType.VarChar, Value = person.Sex.ToString() });
				parameters.Add(new SqlParameter { ParameterName = "@languageId", SqlDbType = SqlDbType.VarChar, Value = person.Language });
				parameters.Add(new SqlParameter { ParameterName = "@locationId", SqlDbType = SqlDbType.VarChar, Value = person.Location });
				parameters.Add(new SqlParameter { ParameterName = "@roleId", SqlDbType = SqlDbType.VarChar, Value = person.Role });

				SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

				return CreateToken(person);
			}

			throw new NullReferenceException("Person object was null!");
		}

		public static string Login(Credentials credentials)
		{
			try
			{
				if (credentials != null)
				{
					Person person;

					if (credentials.Identifier.Contains("@"))
					{
						person = LoginEmail(credentials);
					}
					else
					{
						person = LoginUsername(credentials);
					}

					return CreateToken(person);
				}
			}
			catch (KeyNotFoundException)
			{
				return null!;
			}
			catch (Exception ex)
			{
				return ex.Message;
			}

			throw new NullReferenceException("Credentials object was null!");
		}

		private static Person LoginEmail(Credentials credentials)
		{
			List<SqlParameter> parameters = new List<SqlParameter>();
			string storedProcedure = "SP_Se_LoginEmail";

			parameters.Add(new SqlParameter { ParameterName = "@email", SqlDbType = SqlDbType.VarChar, Value = credentials.Identifier });

			DataTable person = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

			if(person.Rows.Count == 1)
			{
				Person output = Person.ConvertToPerson(person.Rows[0]);

				if (BCrypt.Net.BCrypt.Verify(credentials.Secret, output.Hash))
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

		private static Person LoginUsername(Credentials credentials)
		{
			List<SqlParameter> parameters = new List<SqlParameter>();
			string storedProcedure = "SP_Se_LoginUsername";

			parameters.Add(new SqlParameter { ParameterName = "@username", SqlDbType = SqlDbType.VarChar, Value = credentials.Identifier });

			DataTable person = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

			if (person.Rows.Count == 1)
			{
				Person output = Person.ConvertToPerson(person.Rows[0]);

				if (BCrypt.Net.BCrypt.Verify(credentials.Secret, output.Hash))
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

		private static string HashPassword(Person person)
		{
			string hash = BCrypt.Net.BCrypt.HashPassword(person.Hash);

			return hash;
		}

		private static string CreateToken(Person person)
		{
			List<Claim> claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, ""),
				new Claim(ClaimTypes.Name, person.Username),
				new Claim(ClaimTypes.Email, person.Email),
				new Claim(ClaimTypes.Role, $"{person.Role}")
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
	}
}
