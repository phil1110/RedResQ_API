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
				DataSource = "localhost",
				UserID = "api",
				Password = "password",
				InitialCatalog = "RedResQ"
			};

			ConnectionString = builder.ConnectionString;
		}

		public static string ConnectionString { get; private set; }
	}
}
