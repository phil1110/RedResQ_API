using RedResQ_API.Lib.Exceptions;
using RedResQ_API.Lib.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace RedResQ_API.Lib.Services
{
    public static class AttemptService
    {
        public static long Add(JwtClaims claims, Attempt attempt)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_At_AddAttempt";

            DataTable givenAnswers = new DataTable();

            givenAnswers.Columns.Add("QuizID", typeof(long));
            givenAnswers.Columns.Add("UserID", typeof(long));
            givenAnswers.Columns.Add("QuestionID", typeof(long));
            givenAnswers.Columns.Add("AnswerID", typeof(long));

            foreach (var q in attempt.GivenAnswers)
            {
                givenAnswers.Rows.Add(attempt.QuizId, claims.Id, q.QuestionId, q.AnswerId);
            }

            parameters.Add(new SqlParameter { ParameterName = "@quizId", SqlDbType = SqlDbType.BigInt, Value = attempt.QuizId });
            parameters.Add(new SqlParameter { ParameterName = "@userId", SqlDbType = SqlDbType.BigInt, Value = claims.Id });
            parameters.Add(new SqlParameter { ParameterName = "@data", SqlDbType = SqlDbType.Structured, Value = givenAnswers });

            DataTable idTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (idTable.Rows.Count == 1)
            {
                return Convert.ToInt64(idTable.Rows[0].ItemArray[0]!);
            }

            throw new UnprocessableEntityException();
        }

        public static int GetResult(long attemptId, long quizId, long userId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_As_GetResult";

            parameters.Add(new SqlParameter { ParameterName = "@attemptId", SqlDbType = SqlDbType.BigInt, Value = attemptId });
            parameters.Add(new SqlParameter { ParameterName = "@quizId", SqlDbType = SqlDbType.BigInt, Value = quizId });
            parameters.Add(new SqlParameter { ParameterName = "@userId", SqlDbType = SqlDbType.BigInt, Value = userId });

            DataTable resultTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (resultTable.Rows.Count == 1)
            {
                return Convert.ToInt32(resultTable.Rows[0].ItemArray[0]!);
            }

            throw new NotFoundException();
        }
    }
}
