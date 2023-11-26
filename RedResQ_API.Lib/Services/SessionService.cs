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
		public static string Register(RawUser person)
		{
			if (person != null)
			{
				List<SqlParameter> parameters = new List<SqlParameter>();
				string storedProcedure = "SP_Se_Register";
				person.Hash = HashPassword(person.Hash);

				parameters.Add(new SqlParameter { ParameterName = "@username", SqlDbType = SqlDbType.VarChar, Value = person.Username } );
				parameters.Add(new SqlParameter { ParameterName = "@firstname", SqlDbType = SqlDbType.VarChar, Value = person.FirstName });
				parameters.Add(new SqlParameter { ParameterName = "@lastname", SqlDbType = SqlDbType.VarChar, Value = person.LastName });
				parameters.Add(new SqlParameter { ParameterName = "@email", SqlDbType = SqlDbType.VarChar, Value = person.Email });
				parameters.Add(new SqlParameter { ParameterName = "@birthdate", SqlDbType = SqlDbType.DateTime, Value = person.Birthdate });
				parameters.Add(new SqlParameter { ParameterName = "@hash", SqlDbType = SqlDbType.VarChar, Value = person.Hash });
				parameters.Add(new SqlParameter { ParameterName = "@gender", SqlDbType = SqlDbType.BigInt, Value = person.Gender });
				parameters.Add(new SqlParameter { ParameterName = "@languageId", SqlDbType = SqlDbType.BigInt, Value = person.Language });
				parameters.Add(new SqlParameter { ParameterName = "@locationId", SqlDbType = SqlDbType.BigInt, Value = person.Location });
				parameters.Add(new SqlParameter { ParameterName = "@roleId", SqlDbType = SqlDbType.BigInt, Value = person.Role });

				SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

				return JwtHandler.CreateToken(person);
			}

			throw new NullReferenceException("Person object was null!");
		}

		public static string Login(Credentials credentials)
		{
			try
			{
				if (credentials != null)
				{
					User person;

					if (credentials.Identifier.Contains("@"))
					{
						person = LoginEmail(credentials);
					}
					else
					{
						person = LoginUsername(credentials);
					}

					return JwtHandler.CreateToken(person);
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

		private static User LoginEmail(Credentials credentials)
		{
			List<SqlParameter> parameters = new List<SqlParameter>();
			string storedProcedure = "SP_Se_LoginEmail";

			parameters.Add(new SqlParameter { ParameterName = "@email", SqlDbType = SqlDbType.VarChar, Value = credentials.Identifier });

			DataTable person = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

			if(person.Rows.Count == 1)
			{
				User output = User.ConvertToPerson(person.Rows[0]);

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

		private static User LoginUsername(Credentials credentials)
		{
			List<SqlParameter> parameters = new List<SqlParameter>();
			string storedProcedure = "SP_Se_LoginUsername";

			parameters.Add(new SqlParameter { ParameterName = "@username", SqlDbType = SqlDbType.VarChar, Value = credentials.Identifier });

			DataTable person = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

			if (person.Rows.Count == 1)
			{
				User output = User.ConvertToPerson(person.Rows[0]);

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

		private static string HashPassword(string hash)
		{
			return BCrypt.Net.BCrypt.HashPassword(hash);
		}
	}
}
