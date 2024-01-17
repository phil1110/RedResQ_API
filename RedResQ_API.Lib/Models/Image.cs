using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
	public class Image
	{
		#region Constructor
		
		public Image(long id, string base64)
		{
			Id = id;
            Base64 = base64;
		}

		#endregion

		#region Properties

		public long Id { get; private set; }

		public string Base64 { get; private set; }

        #endregion

        #region Methods

		public static Image ConvertToImage(DataRow row)
		{
            int length = row.ItemArray.Length - 1;

            long id = Convert.ToInt64(row.ItemArray[length--])!;
            string base64 = Convert.ToString(row.ItemArray[length--])!;

            return new Image(id, base64);
        }

        #endregion
    }
}
