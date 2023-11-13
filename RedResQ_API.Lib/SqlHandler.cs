using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib
{
	internal static class SqlHandler
	{
		internal static DataTable ExecuteQuery(string storedProcdedure, SqlParameter[]? parameters = null)
		{
			DataTable? output = null;

			using (var connection = new SqlConnection(Constants.ConnectionString))
			{
				Console.WriteLine(connection.State);

				using (var cmd = new SqlCommand(storedProcdedure, connection))
				{
					cmd.CommandType = CommandType.StoredProcedure;

					if(parameters != null)
					{
						foreach (SqlParameter parameter in parameters)
						{
							cmd.Parameters.Add(parameter);
						}
					}

					connection.Open();

					var reader = cmd.ExecuteReader();

					output!.Load(reader);

					return output;
				}
			}
		}

		internal static int ExecuteNonQuery(string storedProcdedure, SqlParameter[]? parameters = null)
		{
			using (var connection = new SqlConnection(Constants.ConnectionString))
			{
				Console.WriteLine(connection.State);

				using (var cmd = new SqlCommand(storedProcdedure, connection))
				{
					cmd.CommandType = CommandType.StoredProcedure;

					if (parameters != null)
					{
						foreach (SqlParameter parameter in parameters)
						{
							cmd.Parameters.Add(parameter);
						}
					}

					connection.Open();

					int output = cmd.ExecuteNonQuery();

					return output;
				}
			}
		}
	} 
}
