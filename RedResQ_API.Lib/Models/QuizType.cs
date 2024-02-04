using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
    public class QuizType
    {
        #region Constructor

        public QuizType(long id, string name)
        {
            Id = id;
            Name = name;
        }

        #endregion

        #region Properties

        public long Id { get; set; }

        public string Name { get; set; }

        #endregion
    }
}
