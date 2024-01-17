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

        #region Methods

		public static Answer ConvertToAnswer(DataRow row)
		{
            int length = row.ItemArray.Length - 1;

            bool isTrue = Convert.ToBoolean(Convert.ToInt16(row.ItemArray[length--])!);
            string text = Convert.ToString(row.ItemArray[length--])!;
            long id = Convert.ToInt64(row.ItemArray[length--])!;
            long questionId = Convert.ToInt64(row.ItemArray[length--])!;
            long quizId = Convert.ToInt64(row.ItemArray[length--])!;

            return new Answer(quizId, questionId, id, text, isTrue);
        }

        #endregion
    }
}
