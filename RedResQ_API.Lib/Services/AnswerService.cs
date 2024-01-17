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
        public static Answer GetAnswer(JwtClaims claims, long id)
        {
            throw new NotImplementedException(); 
        }

        public static Answer[] GetAnswersForQuestion(JwtClaims claims, long questionId)
        {
            throw new NotImplementedException();
        }

        internal static Answer[] GetAnswersForQuestion(long questionId)
        {
            List<Answer> answers = new List<Answer>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_An_GetAnswersForQuestion";

            parameters.Add(new SqlParameter { ParameterName = "@questionId", SqlDbType = SqlDbType.BigInt, Value = questionId });

            DataTable answerTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if(answerTable.Rows.Count > 0)
            {
                foreach (DataRow row in answerTable.Rows)
                {
                    answers.Add(Answer.ConvertToAnswer(row));
                }
            }

            throw new NotFoundException();
        }
    }
}
