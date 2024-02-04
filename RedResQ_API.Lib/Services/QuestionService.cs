using RedResQ_API.Lib.Models;
using RedResQ_API.Lib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace RedResQ_API.Lib.Services
{
    public static class QuestionService
    {
        public static Question[] Fetch(long quizId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Qn_FetchQuestions";

            parameters.Add(new SqlParameter { ParameterName = "@quizId", SqlDbType = SqlDbType.BigInt, Value = quizId });

            DataTable questionViewTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (questionViewTable.Rows.Count > 0)
            {
                List<Question> questions = new List<Question>();

                foreach (DataRow row in questionViewTable.Rows)
                {
                    questions.Add(Converter.ToQuestion(row.ItemArray.ToList()!, null!));
                }

                return questions.ToArray();
            }

            throw new NotFoundException();
        }

        public static Question Get(long quizId, long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<QuestionViewRow> questionViewRows = new List<QuestionViewRow>();
            string storedProcedure = "SP_Qn_GetQuestion";

            parameters.Add(new SqlParameter { ParameterName = "@quizId", SqlDbType = SqlDbType.BigInt, Value = quizId });
            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

            DataTable questionViewTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (questionViewTable.Rows.Count > 0)
            {
                List<Answer> answerList = new List<Answer>();

                foreach (DataRow row in questionViewTable.Rows)
                {
                    questionViewRows.Add(Converter.ToQuestionViewRow(row.ItemArray.ToList()!));
                }

                var answers = questionViewRows.Select(q => new { q.AnswerQuizID, q.AnswerQuestionID, q.AnswerID, q.AnswerText, q.AnswerIsTrue })
                    .GroupBy(q => new { q.AnswerQuizID, q.AnswerQuestionID, q.AnswerID, q.AnswerText, q.AnswerIsTrue })
                    .Select(group => group.First())
                    .ToList();

                foreach (var a in answers)
                {
                    answerList.Add(new Answer(a.AnswerQuizID, a.AnswerQuestionID, a.AnswerID, a.AnswerText, a.AnswerIsTrue));
                }

                return new Question(questionViewRows[0].QuestionQuizID, questionViewRows[0].QuestionId, questionViewRows[0].QuestionText, answerList.ToArray());
            }

            throw new NotFoundException();
        }

        public static bool Add(Question question)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            DataTable answers = new DataTable();
            string storedProcedure = "SP_Qn_AddQuestion";

            answers.Columns.Add("QuizID", typeof(long));
            answers.Columns.Add("QuestionID", typeof(long));
            answers.Columns.Add("ID", typeof(long));
            answers.Columns.Add("Text", typeof(string));
            answers.Columns.Add("IsTrue", typeof(bool));

            foreach (var a in question.Answers)
            {
                answers.Rows.Add(a.QuizId, a.QuestionId, a.Id, a.Text, a.IsTrue);
            }

            parameters.Add(new SqlParameter { ParameterName = "@quizId", SqlDbType = SqlDbType.BigInt, Value = question.QuizId });
            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = question.Id });
            parameters.Add(new SqlParameter { ParameterName = "@text", SqlDbType = SqlDbType.VarChar, Value = question.Text });
            parameters.Add(new SqlParameter { ParameterName = "@answers", SqlDbType = SqlDbType.Structured, Value = answers });

            int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

            if (rowsAffected > 0)
            {
                return true;
            }

            throw new UnprocessableEntityException();
        }

        public static bool Edit(long quizId, long id, string text)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Qn_EditQuestion";

            parameters.Add(new SqlParameter { ParameterName = "@quizId", SqlDbType = SqlDbType.BigInt, Value = quizId });
            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });
            parameters.Add(new SqlParameter { ParameterName = "@text", SqlDbType = SqlDbType.VarChar, Value = text });

            int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

            if (rowsAffected > 0)
            {
                return true;
            }

            throw new UnprocessableEntityException();
        }

        public static bool Delete(long quizId, long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Qn_DeleteQuestion";

            parameters.Add(new SqlParameter { ParameterName = "@quizId", SqlDbType = SqlDbType.BigInt, Value = quizId });
            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

            int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

            if (rowsAffected > 0)
            {
                return true;
            }

            throw new UnprocessableEntityException();
        }
    }
}
