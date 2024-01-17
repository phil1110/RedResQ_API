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

		public Answer(long id, string text, bool isTrue, long questionId)
		{
			Id = id;
			Text = text;
			IsTrue = isTrue;
			QuestionID = questionId;
		}

		#endregion

		#region Properties

		public long Id { get; private set; }

		public string Text { get; private set; }

		public bool IsTrue { get; private set; }

		public long QuestionID { get; private set; }

        #endregion

        #region Methods

		public static Answer ConvertToAnswer(DataRow row)
		{
            int length = row.ItemArray.Length - 1;

            long questionId = Convert.ToInt64(row.ItemArray[length--])!;
            bool isTrue = Convert.ToBoolean(Convert.ToInt16(row.ItemArray[length--])!);
            string text = Convert.ToString(row.ItemArray[length--])!;
            long id = Convert.ToInt64(row.ItemArray[length--])!;

            return new Answer(id, text, isTrue, questionId);
        }

        #endregion
    }
}
