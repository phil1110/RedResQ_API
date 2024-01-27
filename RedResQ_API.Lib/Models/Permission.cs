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
    }
}
