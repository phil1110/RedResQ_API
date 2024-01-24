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
        public static Question Get(JwtClaims claims, long quizId, long id)
        {
            if(PermissionService.IsPermitted("getQuestion", claims.Role))
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Qn_GetQuestion";

                parameters.Add(new SqlParameter { ParameterName = "@quizId", SqlDbType = SqlDbType.BigInt, Value = quizId });
                parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

                DataTable questionTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

                if(questionTable.Rows.Count == 1)
                {
                    return Question.ConvertToQuestion(questionTable.Rows[0]);
                }
            }

            throw new NotFoundException($"Question (QuizID: {quizId}, ID: {id}) was not found!");
        }


    }
}
