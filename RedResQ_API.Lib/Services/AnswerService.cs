using RedResQ_API.Lib.Models;
using RedResQ_API.Lib.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Services
{
    public static class AnswerService
    {
        public static Answer Get(JwtClaims claims, long quizId, long questionId, long id)
        {
            if(PermissionService.IsPermitted("getAnswer", claims.Role))
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_An_GetAnswer";

                parameters.Add(new SqlParameter { ParameterName = "@quizId", SqlDbType = SqlDbType.BigInt, Value = quizId });
                parameters.Add(new SqlParameter { ParameterName = "@questionId", SqlDbType = SqlDbType.BigInt, Value = questionId });
                parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

                DataTable answerTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

                if (answerTable.Rows.Count == 1)
                {
                    return Answer.ConvertToAnswer(answerTable.Rows[0]);
                }
            }

            throw new NotFoundException();
        }

        public static Answer[] GetForQuestion(JwtClaims claims, long quizId, long questionId)
        {
            if (PermissionService.IsPermitted("getAnswer", claims.Role))
            {
                return GetForQuestion(quizId, questionId);
            }

            throw new NotFoundException();
        }

        internal static Answer[] GetForQuestion(long quizId, long questionId)
        {
            List<Answer> answers = new List<Answer>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_An_GetAnswersForQuestion";

            parameters.Add(new SqlParameter { ParameterName = "@quizId", SqlDbType = SqlDbType.BigInt, Value = quizId });
            parameters.Add(new SqlParameter { ParameterName = "@questionId", SqlDbType = SqlDbType.BigInt, Value = questionId });

            DataTable answerTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if(answerTable.Rows.Count > 0)
            {
                foreach (DataRow row in answerTable.Rows)
                {
                    answers.Add(Answer.ConvertToAnswer(row));
                }

                return answers.ToArray();
            }

            throw new NotFoundException();
        }

        public static Answer[] Search(JwtClaims claims, long quizId, long? questionId, long? id)
        {
            if(PermissionService.IsPermitted("searchAnswer", claims.Role))
            {
                List<Answer> answers = new List<Answer>();
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_An_SearchAnswers";

                parameters.Add(new SqlParameter { ParameterName = "@quizId", SqlDbType = SqlDbType.BigInt, Value = quizId });

                if (questionId.HasValue)
                {
                    parameters.Add(new SqlParameter { ParameterName = "@questionId", SqlDbType = SqlDbType.BigInt, Value = questionId });
                }
                if(id.HasValue)
                {
                    parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });
                }

                DataTable answerTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

                if (answerTable.Rows.Count > 0)
                {
                    foreach (DataRow row in answerTable.Rows)
                    {
                        answers.Add(Answer.ConvertToAnswer(row));
                    }

                    return answers.ToArray();
                }
            }

            throw new NotFoundException();
        }

        public static bool Add(JwtClaims claims, Answer answer)
        {
            if(PermissionService.IsPermitted("addAnswer", claims.Role))
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_An_AddAnswer";

                parameters.Add(new SqlParameter { ParameterName = "@quizId", SqlDbType = SqlDbType.BigInt, Value = answer.QuizId });
                parameters.Add(new SqlParameter { ParameterName = "@questionId", SqlDbType = SqlDbType.BigInt, Value = answer.QuestionId });
                parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = answer.Id });
                parameters.Add(new SqlParameter { ParameterName = "@text", SqlDbType = SqlDbType.BigInt, Value = answer.Text });
                parameters.Add(new SqlParameter { ParameterName = "@isTrue", SqlDbType = SqlDbType.Bit, Value = Convert.ToInt16(answer.IsTrue) });

                int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

                if (rowsAffected == 1)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool Delete(JwtClaims claims, long quizId, long questionId, long id)
        {
            if(PermissionService.IsPermitted("deleteAnswer", claims.Role))
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_An_DeleteAnswer";

                parameters.Add(new SqlParameter { ParameterName = "@quizId", SqlDbType = SqlDbType.BigInt, Value = quizId });
                parameters.Add(new SqlParameter { ParameterName = "@questionId", SqlDbType = SqlDbType.BigInt, Value = questionId });
                parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

                int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

                if (rowsAffected == 1)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool Edit(JwtClaims claims, Answer answer)
        {
            if(PermissionService.IsPermitted("editAnswer", claims.Role))
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_An_EditAnswer";

                parameters.Add(new SqlParameter { ParameterName = "@quizId", SqlDbType = SqlDbType.BigInt, Value = answer.QuizId });
                parameters.Add(new SqlParameter { ParameterName = "@questionId", SqlDbType = SqlDbType.BigInt, Value = answer.QuestionId });
                parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = answer.Id });
                parameters.Add(new SqlParameter { ParameterName = "@text", SqlDbType = SqlDbType.BigInt, Value = answer.Text });
                parameters.Add(new SqlParameter { ParameterName = "@isTrue", SqlDbType = SqlDbType.Bit, Value = Convert.ToInt16(answer.IsTrue) });

                int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

                if (rowsAffected == 1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
