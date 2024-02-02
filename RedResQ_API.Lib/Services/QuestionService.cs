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

            return false;
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

            return false;
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

            return false;
        }
    }
}
