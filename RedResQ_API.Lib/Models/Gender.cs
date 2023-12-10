using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
	public class Gender
	{
		#region Constructor

		public Gender(long id, string name)
		{
			Id = id;
			Name = name;
		}

		#endregion

		#region Properties

		public long Id {  get; private set; }

		public string Name { get; private set; }

        #endregion

        #region Methods

		public static Gender ConvertToGender(DataRow row)
		{
            int length = row.ItemArray.Length - 1;

            string name = Convert.ToString(row.ItemArray[length--])!;
            long id = Convert.ToInt64(row.ItemArray[length--])!;

            return new Gender(id, name);
        }

        #endregion
    }
}
