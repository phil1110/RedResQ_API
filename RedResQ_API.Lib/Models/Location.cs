using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
	public class Location
	{
		#region Constructor

		public Location(int id, string country, string city, string postalCode)
		{
			Id = id;
			Country = country;
			City = city;
			PostalCode = postalCode;
		}

		#endregion

		#region Properties

		public int Id { get; private set; }

		public string Country { get; private set; }

		public string City { get; private set; }

		public string PostalCode { get; private set; }

		#endregion
	}
}
