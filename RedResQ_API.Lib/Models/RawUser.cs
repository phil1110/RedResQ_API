using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
	public class RawUser : IUser
	{
		#region Instance variables

		private string _username;
		private string _firstName;
		private string _lastName;
		private string _email;
		private DateTime _birthdate;
		private string _hash;
		private long _gender;
		private long _language;
		private long _location;
		private long _role;

		#endregion

		#region Constructor

		public RawUser(string username, string firstName, string lastName, string email, DateTime birthdate,
			string hash, long gender, long language, long location, long role)
		{
			Username = username;
			FirstName = firstName;
			LastName = lastName;
			Email = email;
			Birthdate = birthdate;
			Hash = hash;
			Gender = gender;
			Language = language;
			Location = location;
			Role = null!;
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

		public long Gender
		{
			get => _gender;
			private set => _gender = value;
		}

		public long Language
		{
			get => _language;
			private set => _language = value;
		}

		public long Location
		{
			get => _location;
			private set => _location = value;
		}

		public Role Role
		{
			get;
		}

		#endregion
	}
}
