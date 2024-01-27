using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
	public class User
	{
		#region Constructor

		public User(long id, string username, string firstName, string lastName, string email, DateTime birthdate,
			Gender gender, Language language, Location location, Role role)
		{
			Id = id;
			Username = username;
			FirstName = firstName;
			LastName = lastName;
			Email = email;
			Birthdate = birthdate;
			Gender = gender;
			Language = language;
			Location = location;
			Role = role;
		}

		#endregion

		#region Properties

		public long Id { get; private set; }

		public string Username { get; private set; }

		public string FirstName { get; private set; }

		public string LastName { get; private set; }

		public string Email { get; private set; }

		public DateTime Birthdate { get; private set; }

		public Gender Gender { get; private set; }

		public Language Language { get; private set; }

		public Location Location { get; private set; }

		public Role Role { get; private set; }

		#endregion
	}
}
