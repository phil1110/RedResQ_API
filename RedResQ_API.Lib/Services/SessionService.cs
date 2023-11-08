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
		public static Session Login(string identifier, string password, string deviceId)
		{
			Session output;

			if(identifier.Contains("@"))
			{
				output = LoginEmail(identifier, password, deviceId);
			}
			else
			{
				output = LoginUsername(identifier, password, deviceId);
			}

			if(output == null)
			{

			}
		}

		private static Session LoginEmail(string email, string deviceId, string password)
		{
			try
			{
				using (var connection = new SqlConnection(Constants.ConnectionString))
				{
					Console.WriteLine(connection.State);

					var sql = $"exec LoginEmail @email = #email, @deviceId = #deviceId, @password = #password";

					using (var cmd = new SqlCommand(sql, connection))
					{
						cmd.Parameters.Add("#email", SqlDbType.VarChar).Value = email;
						cmd.Parameters.Add("#deviceId", SqlDbType.VarChar).Value = deviceId;
						cmd.Parameters.Add("#password", SqlDbType.VarChar).Value = password;

						connection.Open();

						var reader = cmd.ExecuteReader();
						var output = new DataTable();

						output.Load(reader);

						connection.Close();

						if (output.Rows.Count == 1)
						{
							return ConvertToSession(output);
						}
					}

					sql = $"exec LoginEmail @email = #email, @deviceId = #deviceId, @password = #password";

					using (var cmd = new SqlCommand(sql, connection))
					{
						cmd.Parameters.Add("#email", SqlDbType.VarChar).Value = email;
						cmd.Parameters.Add("#deviceId", SqlDbType.VarChar).Value = deviceId;
						cmd.Parameters.Add("#password", SqlDbType.VarChar).Value = password;

						connection.Open();

						var reader = cmd.ExecuteReader();
						var output = new DataTable();

						output.Load(reader);

						connection.Close();

						if (output.Rows.Count == 1)
						{
							return ConvertToSession(output);
						}
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		private static Session LoginUsername(string username, string deviceId, string password)
		{
			string query = "exec LoginUsername @username = #username, @deviceId = #deviceId, @password = #password";

			List<SqlParameter> parameters = new List<SqlParameter>();
			parameters.Add((SqlParameter)(new SqlParameter("#username", SqlDbType.VarChar).Value = username));
			parameters.Add((SqlParameter)(new SqlParameter("#deviceId", SqlDbType.VarChar).Value = deviceId));
			parameters.Add((SqlParameter)(new SqlParameter("#password", SqlDbType.VarChar).Value = password));

			DataTable output = SqlHandler.ExecuteSelect(query, parameters.ToArray());

			if (output.Rows.Count == 1)
			{
				return ConvertToSession(output, ConvertToPerson(output));
			}
			else
			{
				return null;
			}
		}

		public static Session Register(Session session, string password)
		{
			return null;
		}

		private static Session ConvertToSession(DataTable table, Person person)
		{
			int length = 1;

			return new Session(Convert.ToInt32(table.Rows[0].ItemArray[length - 1]),
				Convert.ToString(table.Rows[0].ItemArray[length]),
				person);
		}

		private static Person ConvertToPerson(DataTable table)
		{
			var length = table.Rows[0].ItemArray.Length - 1;

			var role = new Role(Convert.ToInt32(table.Rows[0].ItemArray[length - 1]),
				Convert.ToString(table.Rows[0].ItemArray[length]));

			length -= 2;

			var loc = new Location(Convert.ToInt32(table.Rows[0].ItemArray[length - 3]),
				Convert.ToString(table.Rows[0].ItemArray[length - 2]),
				Convert.ToString(table.Rows[0].ItemArray[length - 1]),
				Convert.ToString(table.Rows[0].ItemArray[length]));

			length -= 4;

			var lang = new Language(Convert.ToInt32(table.Rows[0].ItemArray[length - 1]),
				Convert.ToString(table.Rows[0].ItemArray[length]));

			length -= 2;

			if (!Enum.TryParse<Sex>(Convert.ToString(table.Rows[0].ItemArray[length]), out var sex))
			{
				return null;
			}

			length--;

			var person = new Person(Convert.ToInt32(table.Rows[0].ItemArray[length - 5]),
				Convert.ToString(table.Rows[0].ItemArray[length - 4]),
				Convert.ToString(table.Rows[0].ItemArray[length - 3]),
				Convert.ToString(table.Rows[0].ItemArray[length - 2]),
				Convert.ToString(table.Rows[0].ItemArray[length - 1]),
				(DateTime)table.Rows[0].ItemArray[length],
				sex, lang, loc, null, role);

			length -= 6;

			return person;
		}
	}
}
