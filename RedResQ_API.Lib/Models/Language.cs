using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
	public class Language
	{
		#region Constructor

		public Language(long id, string name)
		{
			Id = id;
			Name = name;
		}

		#endregion

		#region Properties

		public long Id { get; private set; }

		public string Name { get; private set; }

        #endregion

        #region Methods

		public static Language ConvertToLanguage(DataRow row)
		{
            int length = row.ItemArray.Length - 1;

            string name = Convert.ToString(row.ItemArray[length--])!;
            long id = Convert.ToInt64(row.ItemArray[length--])!;

            return new Language(id, name);
        }

        #endregion
    }
}
