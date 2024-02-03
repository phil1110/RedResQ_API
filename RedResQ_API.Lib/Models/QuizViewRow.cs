using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
    public class QuizViewRow
    {
        #region Constructor

        public QuizViewRow(long quiz_ID, string quiz_Name, int quiz_MaxScore, long quiz_TypeID, long quizType_ID, string quizType_Name, 
            long question_QuizID, long question_ID, string question_text, long answer_QuizID, long answer_QuestionID, long answer_ID, 
            string answer_text, bool answer_isTrue)
        {
            Quiz_ID = quiz_ID;
            Quiz_Name = quiz_Name;
            Quiz_MaxScore = quiz_MaxScore;
            Quiz_TypeID = quiz_TypeID;
            QuizType_ID = quizType_ID;
            QuizType_Name = quizType_Name;
            Question_QuizID = question_QuizID;
            Question_ID = question_ID;
            Question_text = question_text;
            Answer_QuizID = answer_QuizID;
            Answer_QuestionID = answer_QuestionID;
            Answer_ID = answer_ID;
            Answer_text = answer_text;
            Answer_isTrue = answer_isTrue;
        }

        #endregion

        #region Properties

        public long Quiz_ID { get; private set; }

        public string Quiz_Name { get; private set; }

        public int Quiz_MaxScore { get; private set; }

        public long Quiz_TypeID { get; private set; }

        public long QuizType_ID { get; private set; }

        public string QuizType_Name { get; private set; }

        public long Question_QuizID { get; private set; }

        public long Question_ID { get; private set; }

        public string Question_text { get; private set; }

        public long Answer_QuizID { get; private set; }

        public long Answer_QuestionID { get; private set; }

        public long Answer_ID { get; private set; }

        public string Answer_text { get; private set; }

        public bool Answer_isTrue { get; private set; }

        #endregion
    }
}
