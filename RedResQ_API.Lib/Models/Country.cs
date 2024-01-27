using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
	public class Country
	{
		#region Constructor

		public Country(long id, string countryName)
		{
			Id = id;
			CountryName = countryName;
		}

		#endregion

		#region Properties

		public long Id { get; private set; }

		public string CountryName { get; private set; }

        #endregion
    }
}
