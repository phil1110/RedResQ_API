using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
	public class Question
	{
		#region Constructor

		public Question(int id, string text, Answer[] answers)
		{
			Id = id;
			Text = text;
			Answers = answers;
		}

		#endregion

		#region Properties

		public int Id { get; private set; }

		public string Text { get; private set; }

		public Answer[] Answers { get; private set; }

		#endregion
	}
}
