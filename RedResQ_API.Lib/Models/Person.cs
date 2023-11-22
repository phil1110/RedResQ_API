using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
	public class Person
	{
		#region Instance variables

		private string _username;
		private string _firstName;
		private string _lastName;
		private string _email;
		private DateTime _birthdate;
		private string _hash;
		private Sex _sex;
		private int _language;
		private int _location;
		private int _settings;
		private int _role;

		#endregion

		#region Constructor

		public Person(string username, string firstName, string lastName, string email, DateTime birthdate,
			string hash, Sex sex, int language, int location, int settings, int role)
		{
			Username = username;
			FirstName = firstName;
			LastName = lastName;
			Email = email;
			Birthdate = birthdate;
			Hash = hash;
			Sex = sex;
			Language = language;
			Location = location;
			Settings = settings;
			Role = role;
		}

		#endregion

		#region Properties

		public string Username
		{
			get => _username;
			private set => _username = value;
		}

		public string FirstName
		{
			get => _firstName;
			private set => _firstName = value;
		}

		public string LastName
		{
			get => _lastName;
			private set => _lastName = value;
		}

		public string Email
		{
			get => _email;
			private set => _email = value;
		}

		public DateTime Birthdate
		{
			get => _birthdate;
			private set => _birthdate = value;
		}

		public string Hash
		{
			get => _hash;
			internal set => _hash = value;
		}

		public Sex Sex
		{
			get => _sex;
			private set => _sex = value;
		}

		public int Language
		{
			get => _language;
			private set => _language = value;
		}

		public int Location
		{
			get => _location;
			private set => _location = value;
		}

		public int Settings
		{
			get => _settings;
			private set => _settings = value;
		}

		public int Role
		{
			get => _role;
			private set => _role = value;
		}

		#endregion

		#region Methods

		internal static Person ConvertToPerson(DataRow row)
		{
			int length = row.ItemArray.Length - 1;

			int role = Convert.ToInt32(row.ItemArray[length--]);

			int loc = Convert.ToInt32(row.ItemArray[length--]);

			int lang = Convert.ToInt32(row.ItemArray[length--]);

			if (!Enum.TryParse(Convert.ToString(row.ItemArray[length--]), out Sex sex))
			{
				return null!;
			}

			string hash = Convert.ToString(row.ItemArray[length--])!;

			DateTime date = (DateTime)row.ItemArray[length--]!;

			string email = Convert.ToString(row.ItemArray[length--])!;

			string lastName = Convert.ToString(row.ItemArray[length--])!;

			string firstName = Convert.ToString(row.ItemArray[length--])!;

			string username = Convert.ToString(row.ItemArray[length--])!;

			return new Person(username, firstName, lastName, email, date, hash, sex, lang, loc, -1,role);
		}

		#endregion
	}
}
