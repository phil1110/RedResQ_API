using RedResQ_API.Lib.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Services
{
	public class UserService
	{
		private static Session ConvertToSession(DataTable table, Person person)
		{
			int length = 1;

			return new Session(Convert.ToInt32(table.Rows[0].ItemArray[length - 1]),
				Convert.ToString(table.Rows[0].ItemArray[length])!,
				person);
		}

		private static Person ConvertToPerson(DataTable table)
		{
			var length = table.Rows[0].ItemArray.Length - 1;

			var role = new Role(Convert.ToInt32(table.Rows[0].ItemArray[length - 1]),
				Convert.ToString(table.Rows[0].ItemArray[length])!);

			length -= 2;

			var loc = new Location(Convert.ToInt32(table.Rows[0].ItemArray[length - 3]),
				Convert.ToString(table.Rows[0].ItemArray[length - 2])!,
				Convert.ToString(table.Rows[0].ItemArray[length - 1])!,
				Convert.ToString(table.Rows[0].ItemArray[length])!);

			length -= 4;

			var lang = new Language(Convert.ToInt32(table.Rows[0].ItemArray[length - 1]),
				Convert.ToString(table.Rows[0].ItemArray[length])!);

			length -= 2;

			if (!Enum.TryParse<Sex>(Convert.ToString(table.Rows[0].ItemArray[length]), out var sex))
			{
				return null!;
			}

			length--;

			var person = new Person(Convert.ToInt32(table.Rows[0].ItemArray[length - 6]),
				Convert.ToString(table.Rows[0].ItemArray[length - 5])!,
				Convert.ToString(table.Rows[0].ItemArray[length - 4])!,
				Convert.ToString(table.Rows[0].ItemArray[length - 3])!,
				Convert.ToString(table.Rows[0].ItemArray[length - 2])!,
				(DateTime)table.Rows[0].ItemArray[length - 1]!,
				Convert.ToString(table.Rows[0].ItemArray[length])!,
				sex, lang, loc, null!, role);

			length -= 6;

			return person;
		}
	}
}
