using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
    public class QuestionViewRow
    {
        #region Constructor

        public QuestionViewRow(long questionQuizID, long questionId, string questionText, long answerQuizID, 
            long answerQuestionID, long answerID, string answerText, bool answerIsTrue)
        {
            QuestionQuizID = questionQuizID;
            QuestionId = questionId;
            QuestionText = questionText;
            AnswerQuizID = answerQuizID;
            AnswerQuestionID = answerQuestionID;
            AnswerID = answerID;
            AnswerText = answerText;
            AnswerIsTrue = answerIsTrue;
        }

        #endregion

        #region Properties

        public long QuestionQuizID { get; private set; }

        public long QuestionId { get; private set; }

        public string QuestionText { get; private set; }

        public long AnswerQuizID { get; private set; }

        public long AnswerQuestionID { get; private set; }

        public long AnswerID { get; private set; }

        public string AnswerText { get; private set; }

        public bool AnswerIsTrue { get; private set; }

        #endregion
    }
}
