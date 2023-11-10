using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib
{
	public static class Constants
	{
		public const int SessionValidity = 30;

		static Constants()
		{
			var builder = new SqlConnectionStringBuilder
			{
				DataSource = Environment.GetEnvironmentVariable("API_DB_DS"),
				UserID = Environment.GetEnvironmentVariable("API_DB_UID"),
				Password = Environment.GetEnvironmentVariable("API_DB_PWD"),
				InitialCatalog = Environment.GetEnvironmentVariable("API_DB_IC")
			};

			ConnectionString = builder.ConnectionString;
		}

		public static string ConnectionString { get; private set; }
	}
}
