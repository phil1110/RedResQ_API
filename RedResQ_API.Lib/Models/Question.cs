using RedResQ_API.Lib.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
	public class Question
	{
		#region Constructor

		public Question(long quizId, long id, string text, Answer[] answers)
		{
			QuizId = quizId;
			Id = id;
			Text = text;
			Answers = answers;
		}

        #endregion

        #region Properties

        public long QuizId { get; private set; }

        public long Id { get; private set; }

		public string Text { get; private set; }

		public Answer[] Answers { get; private set; }

        #endregion
    }
}
