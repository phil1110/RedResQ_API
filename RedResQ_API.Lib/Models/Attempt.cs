using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
    public class Attempt
    {
        #region Constructor

        public Attempt(long quizId, long userId, GivenAnswer[] givenAnswers)
        {
            QuizId = quizId;
            UserId = userId;
            GivenAnswers = givenAnswers;
        }

        #endregion

        #region Properties

        public long QuizId { get; private set; }

        public long UserId { get; private set; }

        public GivenAnswer[] GivenAnswers { get; private set; }

        #endregion
    }
}
