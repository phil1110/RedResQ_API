using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
    public class HazardType
    {
        #region Constructor

        public HazardType(int id, string name)
        {
            Id = id;
            Name = name;
        }

        #endregion

        #region Properties

        public int Id { get; private set; }

        public string Name { get; private set; }

        #endregion
    }
}
