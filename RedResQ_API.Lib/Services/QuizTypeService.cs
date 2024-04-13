using RedResQ_API.Lib.Exceptions;
using RedResQ_API.Lib.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RedResQ_API.Lib.Services
{
    public static class QuizTypeService
    {
        public static QuizType[] Fetch(string? name)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Qt_FetchQuizTypes";

            if(name != null)
            {
                parameters.Add(new SqlParameter { ParameterName = "@name", SqlDbType = SqlDbType.VarChar, Value = name });
            }

            DataTable quizTypeTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (quizTypeTable.Rows.Count > 0)
            {
                List<QuizType> quizTypes = new List<QuizType>();

                foreach (DataRow row in quizTypeTable.Rows)
                {
                    quizTypes.Add(Converter.ToQuizType(row.ItemArray.ToList()!));
                }

                return quizTypes.ToArray();
            }

            throw new NotFoundException();
        }

        public static QuizType Get(long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Qt_GetQuizType";

            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

            DataTable quizTypeTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (quizTypeTable.Rows.Count == 1)
            {
                return Converter.ToQuizType(quizTypeTable.Rows[0].ItemArray.ToList()!);
            }

            throw new NotFoundException();
        }

        public static bool Add(string name)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Qt_AddQuizType";

            parameters.Add(new SqlParameter { ParameterName = "@name", SqlDbType = SqlDbType.VarChar, Value = name });

            int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

            if (rowsAffected > 0)
            {   
                return true;
            }

            throw new UnprocessableEntityException();
        }

        public static bool Edit(QuizType quizType)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Qt_EditQuizType";

            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = quizType.Id });
            parameters.Add(new SqlParameter { ParameterName = "@name", SqlDbType = SqlDbType.VarChar, Value = quizType.Name });

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
            string storedProcedure = "SP_Qt_DeleteQuizType";

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
