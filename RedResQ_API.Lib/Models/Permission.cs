using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
    public class Permission
    {
        #region Constructor

        public Permission(string name, Role requiredRole)
        {
            Name = name;
            RequiredRole = requiredRole;
        }

        #endregion

        #region Properties

        public string Name { get; private set; }

        public Role RequiredRole { get; private set; }

        #endregion

        #region Methods

        public static Permission ConvertToPermission(DataRow row)
        {
            int length = row.ItemArray.Length - 1;

            string roleName = Convert.ToString(row.ItemArray[length--])!;
            long roleId = Convert.ToInt64(row.ItemArray[length--])!;

            string permName = Convert.ToString(row.ItemArray[length--])!;

            Role role = new Role(roleId, roleName);
            return new Permission(permName, role);
        }

        #endregion
    }
}
