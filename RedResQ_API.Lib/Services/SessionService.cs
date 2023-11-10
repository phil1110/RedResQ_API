using Microsoft.VisualBasic;
using RedResQ_API.Lib.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Services
{
	public static class SessionService
	{
		public static Person Register(Person person)
		{
			if (person != null)
			{
				List<SqlParameter> parameters = new List<SqlParameter>();
				string query = "exec Register @username = #username, @firstname = #firstname, @lastname = #lastname, " +
					"@email = #email, @birthdate = #birthdate, @hash = #hash, @sex = #sex, @languageId = #languageId, " +
					"@locationId = #locationId, @roleId = #roleId";
				person.Hash = HashPassword(person);

				parameters.Add((SqlParameter)(new SqlParameter("#username", SqlDbType.VarChar).Value = person.Username));
				parameters.Add((SqlParameter)(new SqlParameter("#firstname", SqlDbType.VarChar).Value = person.FirstName));
				parameters.Add((SqlParameter)(new SqlParameter("#lastname", SqlDbType.VarChar).Value = person.LastName));
				parameters.Add((SqlParameter)(new SqlParameter("#email", SqlDbType.VarChar).Value = person.Email));
				parameters.Add((SqlParameter)(new SqlParameter("#birthdate", SqlDbType.Date).Value = person.Birthdate));
				parameters.Add((SqlParameter)(new SqlParameter("#hash", SqlDbType.VarChar).Value = person.Hash));
				parameters.Add((SqlParameter)(new SqlParameter("#sex", SqlDbType.VarChar).Value = person.Sex));
				parameters.Add((SqlParameter)(new SqlParameter("#languageId", SqlDbType.Int).Value = person.Language.Id));
				parameters.Add((SqlParameter)(new SqlParameter("#locationId", SqlDbType.Int).Value = person.Location.Id));
				parameters.Add((SqlParameter)(new SqlParameter("#roleId", SqlDbType.Int).Value = person.Role.Id));

				SqlHandler.ExecuteNonQuery(query, parameters.ToArray());

				return person;
			}

			throw new NullReferenceException("Person object was null!");
		}

		public static Person Login(Credentials credentials)
		{
			if(credentials != null)
			{
				if (credentials.Identifier.Contains("@"))
				{
					return LoginEmail(credentials);
				}

				return LoginUsername(credentials);
			}

			throw new NullReferenceException("Credentials object was null!");
		}

		private static Person LoginEmail(Credentials credentials)
		{

			throw new NotImplementedException();
		}

		private static Person LoginUsername(Credentials credentials)
		{
			throw new NotImplementedException();
		}

		private static string HashPassword(Person person)
		{
			string hash = string.Empty;

			return person.Hash;
		}
	}
}
