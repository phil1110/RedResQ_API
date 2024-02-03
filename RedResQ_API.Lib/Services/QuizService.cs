using RedResQ_API.Lib.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedResQ_API.Lib.Exceptions;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace RedResQ_API.Lib.Services
{
    public static class QuizService
    {
        public static Quiz[] Fetch(long? id, int? amount)
        {
            List<Quiz> quizzes = new List<Quiz>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Qz_FetchQuizzes";

            if (amount > 100)
            {
                amount = 100;
            }
            if(id.HasValue)
            {
                parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id.Value });
            }
            if (amount.HasValue)
            {
                parameters.Add(new SqlParameter { ParameterName = "@amount", SqlDbType = SqlDbType.Int, Value = amount });
            }
            
            DataTable quizTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            foreach (DataRow row in quizTable.Rows)
            {
                QuizType quizType = Converter.ToQuizType(row.ItemArray.Skip(3).Take(2).ToList()!);

                quizzes.Add(Converter.ToQuiz(row.ItemArray.ToList()!, null!, quizType));
            }

            if (quizzes.Count > 0)
            {
                return quizzes.ToArray();
            }

            throw new NotFoundException("No Quizzes were found!");
        }


        public static Quiz Get(long id)
        {
            List<QuizViewRow> quizViewRows = new List<QuizViewRow>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Qz_GetQuiz";

            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

            DataTable quizTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (quizTable.Rows.Count > 0)
            {
                List<Question> questions = new List<Question>();

                foreach (DataRow row in quizTable.Rows)
                {
                    quizViewRows.Add(Converter.ToQuizViewRow(row.ItemArray.ToList()!));
                }

                #region LINQ Segments

                var answerList = quizViewRows.Select(q => new { q.Answer_QuizID, q.Answer_QuestionID, q.Answer_ID, q.Answer_text, q.Answer_isTrue })
                    .GroupBy(q => new { q.Answer_QuizID, q.Answer_QuestionID, q.Answer_ID, q.Answer_text, q.Answer_isTrue })
                    .Select(group => group.First())
                    .ToList();

                var questionList = quizViewRows.Select(q => new { q.Question_QuizID, q.Question_ID, q.Question_text })
                    .GroupBy(q => new { q.Question_QuizID, q.Question_ID, q.Question_text })
                    .Select(group => group.First())
                    .ToList();

                var quizTypes = quizViewRows.GroupBy(q => new { q.QuizType_ID, q.QuizType_Name })
                    .SelectMany(group => group.Select(q => new { q.QuizType_ID, q.QuizType_Name }))
                    .ToList();

                var quizData = quizViewRows.Select(q => new { q.Quiz_ID, q.Quiz_Name, q.Quiz_MaxScore, q.Quiz_TypeID })
                    .GroupBy(q => new { q.Quiz_ID, q.Quiz_Name, q.Quiz_MaxScore, q.Quiz_TypeID })
                    .Select(group => group.First())
                    .ToList();

                #endregion

                foreach (var question in questionList)
                {
                    List<Answer> answers = new List<Answer>();
                    var answersForQuestion = answerList.Where(q => q.Answer_QuizID == question.Question_QuizID && q.Answer_QuestionID == question.Question_ID).ToList();

                    foreach (var answer in answersForQuestion)
                    {
                        answers.Add(new Answer(answer.Answer_QuizID, answer.Answer_QuestionID, answer.Answer_ID, answer.Answer_text, answer.Answer_isTrue));
                    }

                    questions.Add(new Question(question.Question_QuizID, question.Question_ID, question.Question_text, answers.ToArray()));
                }

                QuizType quizType = new QuizType(quizTypes[0].QuizType_ID, quizTypes[0].QuizType_Name);

                return new Quiz(quizData[0].Quiz_ID, quizData[0].Quiz_Name, quizData[0].Quiz_MaxScore, questions.ToArray(), quizType);
            }

            throw new NotFoundException();
        }

        public static bool Add(Quiz quiz)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Qz_AddQuiz";

            DataTable questions = new DataTable();
            DataTable answers = new DataTable();

            questions.Columns.Add("QuizID", typeof(long));
            questions.Columns.Add("ID", typeof(long));
            questions.Columns.Add("Text", typeof(string));

            answers.Columns.Add("QuizID", typeof(long));
            answers.Columns.Add("QuestionID", typeof(long));
            answers.Columns.Add("ID", typeof(long));
            answers.Columns.Add("Text", typeof(string));
            answers.Columns.Add("IsTrue", typeof(bool));

            foreach (var q in quiz.Questions)
            {
                questions.Rows.Add(q.QuizId, q.Id, q.Text);

                foreach (var a in q.Answers)
                {
                    answers.Rows.Add(a.QuizId, a.QuestionId, a.Id, a.Text, a.IsTrue);
                }
            }

            parameters.Add(new SqlParameter { ParameterName = "@name", SqlDbType = SqlDbType.VarChar, Value = quiz.Name });
            parameters.Add(new SqlParameter { ParameterName = "@maxScore", SqlDbType = SqlDbType.Int, Value = quiz.MaxScore });
            parameters.Add(new SqlParameter { ParameterName = "@TypeID", SqlDbType = SqlDbType.BigInt, Value = quiz.Type.Id });
            parameters.Add(new SqlParameter { ParameterName = "@Questions", SqlDbType = SqlDbType.Structured, Value = questions });
            parameters.Add(new SqlParameter { ParameterName = "@Answers", SqlDbType = SqlDbType.Structured, Value = answers });

            int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

            if (rowsAffected > 0)
            {
                return true;
            }

            throw new UnprocessableEntityException();
        }

        public static bool Edit(long id, string? name, int? maxScore, long? typeId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Qz_EditQuiz";

            if(name != null)
            {
                parameters.Add(new SqlParameter { ParameterName = "@name", SqlDbType = SqlDbType.VarChar, Value = name });
            }
            if(maxScore.HasValue)
            {

                parameters.Add(new SqlParameter { ParameterName = "@maxScore", SqlDbType = SqlDbType.Int, Value = maxScore });
            }
            if(typeId.HasValue)
            {
                parameters.Add(new SqlParameter { ParameterName = "@TypeID", SqlDbType = SqlDbType.BigInt, Value = typeId});
            }

            int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

            if (rowsAffected > 0)
            {
                return true;
            }

            throw new UnprocessableEntityException();
        }

        public static bool Delete(long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Qz_DeleteQuiz";

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
