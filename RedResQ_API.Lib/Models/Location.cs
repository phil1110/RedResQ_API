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

        #region Methods

		public static Location ConvertToLocation(DataRow row)
		{
            int length = row.ItemArray.Length - 1;

            string countryName = Convert.ToString(row.ItemArray[length--])!;
            long countryId = Convert.ToInt64(row.ItemArray[length--])!;

			string postalCode = Convert.ToString(row.ItemArray[length--])!;
			string city = Convert.ToString(row.ItemArray[length--])!;
			long id = Convert.ToInt64(row.ItemArray[length--])!;

            Country country = new Country(countryId, countryName);
			return new Location(id, city, postalCode, country);
        }

        #endregion
    }
}
