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
		public static Session Login(string identifier, string password)
		{
			// WIP - Regex needed to decide between Login Email or Login Username
			return null;
		}

		private static Session LoginEmail(string email, string password)
		{
			try
			{
				using (var connection = new SqlConnection(Constants.ConnectionString))
				{
					Console.WriteLine(connection.State);

					var sql = $"select s.* from VSession as s " +
							  $"left join Person p " +
							  $"on s.PersonID = p.ID " +
							  $"where s.email = @email) " +
							  $"and p.password = @password";

					using (var cmd = new SqlCommand(sql, connection))
					{
						cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
						cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = password;

						connection.Open();

						var reader = cmd.ExecuteReader();
						var output = new DataTable();

						output.Load(reader);

						connection.Close();

						if (output.Rows.Count == 1)
						{
							var length = output.Rows[0].ItemArray.Length - 1;

							var role = new Role(Convert.ToInt32(output.Rows[0].ItemArray[length - 1]),
								Convert.ToString(output.Rows[0].ItemArray[length]));

							length -= 2;

							var loc = new Location(Convert.ToInt32(output.Rows[0].ItemArray[length - 3]),
								Convert.ToString(output.Rows[0].ItemArray[length - 2]),
								Convert.ToString(output.Rows[0].ItemArray[length - 1]),
								Convert.ToString(output.Rows[0].ItemArray[length]));

							length -= 4;

							var lang = new Language(Convert.ToInt32(output.Rows[0].ItemArray[length - 1]),
								Convert.ToString(output.Rows[0].ItemArray[length]));

							length -= 2;

							if (!Enum.TryParse<Sex>(Convert.ToString(output.Rows[0].ItemArray[length]), out var sex))
							{
								return null;
							}

							length--;

							var person = new Person(Convert.ToInt32(output.Rows[0].ItemArray[length - 5]),
								Convert.ToString(output.Rows[0].ItemArray[length - 4]),
								Convert.ToString(output.Rows[0].ItemArray[length - 3]),
								Convert.ToString(output.Rows[0].ItemArray[length - 2]),
								Convert.ToString(output.Rows[0].ItemArray[length - 1]),
								(DateTime)output.Rows[0].ItemArray[length],
								sex,
								lang, loc, null, role);

							length -= 6;

							return new Session(Convert.ToInt32(output.Rows[0].ItemArray[length - 1]),
								Convert.ToString(output.Rows[0].ItemArray[length]),
								person);
						}
						else
						{
							return null;
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

		private static Session LoginUsername(string username, string password)
		{
			try
			{
				using (var connection = new SqlConnection(Constants.ConnectionString))
				{
					Console.WriteLine(connection.State);

					var sql = $"select s.* from VSession as s " +
							  $"left join Person p " +
							  $"on s.PersonID = p.ID " +
							  $"where s.username = @username " +
							  $"and p.password = @password";

					using (var cmd = new SqlCommand(sql, connection))
					{
						cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = username;
						cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = password;

						connection.Open();

						var reader = cmd.ExecuteReader();
						var output = new DataTable();

						output.Load(reader);

						connection.Close();

						if (output.Rows.Count == 1)
						{
							var length = output.Rows[0].ItemArray.Length - 1;

							var role = new Role(Convert.ToInt32(output.Rows[0].ItemArray[length - 1]),
								Convert.ToString(output.Rows[0].ItemArray[length]));

							length -= 2;

							var loc = new Location(Convert.ToInt32(output.Rows[0].ItemArray[length - 3]),
								Convert.ToString(output.Rows[0].ItemArray[length - 2]),
								Convert.ToString(output.Rows[0].ItemArray[length - 1]),
								Convert.ToString(output.Rows[0].ItemArray[length]));

							length -= 4;

							var lang = new Language(Convert.ToInt32(output.Rows[0].ItemArray[length - 1]),
								Convert.ToString(output.Rows[0].ItemArray[length]));

							length -= 2;

							if (!Enum.TryParse<Sex>(Convert.ToString(output.Rows[0].ItemArray[length]), out var sex))
							{
								return null;
							}

							length--;

							var person = new Person(Convert.ToInt32(output.Rows[0].ItemArray[length - 5]),
								Convert.ToString(output.Rows[0].ItemArray[length - 4]),
								Convert.ToString(output.Rows[0].ItemArray[length - 3]),
								Convert.ToString(output.Rows[0].ItemArray[length - 2]),
								Convert.ToString(output.Rows[0].ItemArray[length - 1]),
								(DateTime)output.Rows[0].ItemArray[length],
								sex,
								lang, loc, null, role);

							length -= 6;

							return new Session(Convert.ToInt32(output.Rows[0].ItemArray[length - 1]),
								Convert.ToString(output.Rows[0].ItemArray[length]),
								person);
						}
						else
						{
							return null;
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
	}
}
