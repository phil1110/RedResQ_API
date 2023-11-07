using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
	public class Person
	{
		#region Instance variables

		private int _id;
		private string _username;
		private string _firstName;
		private string _lastName;
		private string _email;
		private DateTime _birthdate;
		private Sex _sex;
		private Language _language;
		private Location _location;
		private Settings _settings;
		private Role _role;

		#endregion

		#region Constructor

		public Person(int id, string username, string firstName, string lastName, string email, DateTime birthdate,
			Sex sex, Language language, Location location, Settings settings, Role role)
		{
			Id = id;
			Username = username;
			FirstName = firstName;
			LastName = lastName;
			Email = email;
			Birthdate = birthdate;
			Sex = sex;
			Language = language;
			Location = location;
			Settings = settings;
			Role = role;
		}

		#endregion

		#region Properties

		public int Id
		{
			get => _id;
			private set => _id = value;
		}

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

		public Sex Sex
		{
			get => _sex;
			private set => _sex = value;
		}

		public Language Language
		{
			get => _language;
			private set => _language = value;
		}

		public Location Location
		{
			get => _location;
			private set => _location = value;
		}

		public Settings Settings
		{
			get => _settings;
			private set => _settings = value;
		}

		public Role Role
		{
			get => _role;
			private set => _role = value;
		}

		#endregion
	}
}
