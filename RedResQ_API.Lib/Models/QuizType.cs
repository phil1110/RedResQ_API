using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
    public class QuizType
    {
        #region Constructor

        public QuizType(long id, string name, QuizTypeStage[] stages)
        {
            Id = id;
            Name = name;
            Stages = stages;
        }

        #endregion

        #region Properties

        public long Id { get; private set; }

        public string Name { get; private set; }

        public QuizTypeStage[] Stages { get; private set; }

        #endregion
    }
}
