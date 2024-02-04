using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
    public class GivenAnswer
    {
        #region Constructor
        public GivenAnswer(long questionId, long answerId)
        {
            QuestionId = questionId;
            AnswerId = answerId;
        }

        #endregion

        #region Properties

        public long QuestionId { get; private set; }

        public long AnswerId { get; private set; }

        #endregion
    }
}
