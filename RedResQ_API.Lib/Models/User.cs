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

		#region Methods

		internal static User ConvertToPerson(DataRow row)
		{
			int length = row.ItemArray.Length - 1;

			string roleName = Convert.ToString(row.ItemArray[length--])!;
			long roleId = Convert.ToInt64(row.ItemArray[length--])!;

			string countryName = Convert.ToString(row.ItemArray[length--])!;
			long countryId = Convert.ToInt64(row.ItemArray[length--])!;

			string postalCode = Convert.ToString(row.ItemArray[length--])!;
			string city = Convert.ToString(row.ItemArray[length--])!;
			long locationId = Convert.ToInt64(row.ItemArray[length--])!;

			string languageName = Convert.ToString(row.ItemArray[length--])!;
			long languageId = Convert.ToInt64(row.ItemArray[length--])!;

			string genderName = Convert.ToString(row.ItemArray[length--])!;
			long genderId = Convert.ToInt64(row.ItemArray[length--])!;

			DateTime birthdate = (DateTime)row.ItemArray[length--]!;
			string email = Convert.ToString(row.ItemArray[length--])!;
			string lastName = Convert.ToString(row.ItemArray[length--])!;
			string firstName = Convert.ToString(row.ItemArray[length--])!;
			string username = Convert.ToString(row.ItemArray[length--])!;
			long id = Convert.ToInt64(row.ItemArray[length--])!;

			Role role = new Role(roleId, roleName);
			Country country = new Country(countryId, countryName);
			Location location = new Location(locationId, city, postalCode, country);
			Language language = new Language(languageId, languageName);
			Gender gender = new Gender(genderId, genderName);

			return new User(id, username, firstName, lastName, email, birthdate, gender, language, location, role);
		}

		#endregion
	}
}
