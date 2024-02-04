using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
	public class Answer
	{
		#region Constructor

		public Answer(long quizId, long questionId, long id, string text, bool isTrue)
        {
            QuizId = quizId;
            QuestionId = questionId;
            Id = id;
			Text = text;
			IsTrue = isTrue;
		}

        #endregion

        #region Properties

        public long QuizId { get; private set; }

        public long QuestionId { get; private set; }

        public long Id { get; private set; }

		public string Text { get; private set; }

		public bool IsTrue { get; private set; }

        #endregion
    }
}
