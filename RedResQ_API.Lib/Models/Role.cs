using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
	public class Role
	{
		#region Constructor

		public Role(long id, string name)
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

		internal static Role ConvertToRole(DataRow row)
		{
			int length = row.ItemArray.Length - 1;

			string roleName = Convert.ToString(row.ItemArray[length--])!;
			long roleId = Convert.ToInt64(row.ItemArray[length--]);

			return new Role(roleId, roleName);
		}

		#endregion
	}
}
