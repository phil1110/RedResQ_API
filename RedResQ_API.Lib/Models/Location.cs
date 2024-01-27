using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
	public class Location
	{
		#region Constructor

		public Location(long id, string city, string postalCode, Country country)
		{
			Id = id;
			City = city;
			PostalCode = postalCode;
			Country = country;
		}

		#endregion

		#region Properties

		public long Id { get; private set; }

		public string City { get; private set; }

		public string PostalCode { get; private set; }

		public Country Country { get; private set; }

        #endregion
    }
}
