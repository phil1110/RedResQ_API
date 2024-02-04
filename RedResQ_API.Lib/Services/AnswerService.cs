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
        public static Answer[] Fetch(long quizId, long questionId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_An_FetchAnswers";

            parameters.Add(new SqlParameter { ParameterName = "@quizId", SqlDbType = SqlDbType.BigInt, Value = quizId });
            parameters.Add(new SqlParameter { ParameterName = "@questionId", SqlDbType = SqlDbType.BigInt, Value = questionId });

            DataTable answerTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (answerTable.Rows.Count > 0)
            {
                List<Answer> answer = new List<Answer>();

                foreach (DataRow row in answerTable.Rows)
                {
                    answer.Add(Converter.ToAnswer(row.ItemArray.ToList()!));
                }

                return answer.ToArray();
            }

            throw new NotFoundException();
        }

        public static Answer Get(long quizId, long questionId, long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_An_GetAnswer";

            parameters.Add(new SqlParameter { ParameterName = "@quizId", SqlDbType = SqlDbType.BigInt, Value = quizId });
            parameters.Add(new SqlParameter { ParameterName = "@questionId", SqlDbType = SqlDbType.BigInt, Value = questionId });
            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

            DataTable answerTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (answerTable.Rows.Count == 1)
            {
                return Converter.ToAnswer(answerTable.Rows[0].ItemArray.ToList()!);
            }

            throw new NotFoundException();
        }

        public static bool Add(Answer answer)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_An_AddAnswer";

            parameters.Add(new SqlParameter { ParameterName = "@quizId", SqlDbType = SqlDbType.BigInt, Value = answer.QuizId });
            parameters.Add(new SqlParameter { ParameterName = "@questionId", SqlDbType = SqlDbType.BigInt, Value = answer.QuestionId });
            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = answer.Id });
            parameters.Add(new SqlParameter { ParameterName = "@text", SqlDbType = SqlDbType.VarChar, Value = answer.Text });
            parameters.Add(new SqlParameter { ParameterName = "@isTrue", SqlDbType = SqlDbType.Bit, Value = Convert.ToInt16(answer.IsTrue) });

            int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

            if(rowsAffected > 0)
            {
                return true;
            }

            throw new UnprocessableEntityException();
        }

        public static bool Edit(Answer answer)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_An_EditAnswer";

            parameters.Add(new SqlParameter { ParameterName = "@quizId", SqlDbType = SqlDbType.BigInt, Value = answer.QuizId });
            parameters.Add(new SqlParameter { ParameterName = "@questionId", SqlDbType = SqlDbType.BigInt, Value = answer.QuestionId });
            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = answer.Id });
            parameters.Add(new SqlParameter { ParameterName = "@text", SqlDbType = SqlDbType.VarChar, Value = answer.Text });
            parameters.Add(new SqlParameter { ParameterName = "@isTrue", SqlDbType = SqlDbType.Bit, Value = Convert.ToInt16(answer.IsTrue) });

            int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

            if (rowsAffected > 0)
            {
                return true;
            }

            throw new UnprocessableEntityException();
        }

        public static bool Delete(long quizId, long questionId, long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_An_DeleteAnswer";

            parameters.Add(new SqlParameter { ParameterName = "@quizId", SqlDbType = SqlDbType.BigInt, Value = quizId });
            parameters.Add(new SqlParameter { ParameterName = "@questionId", SqlDbType = SqlDbType.BigInt, Value = questionId });
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
