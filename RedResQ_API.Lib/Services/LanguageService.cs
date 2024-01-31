using RedResQ_API.Lib.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using RedResQ_API.Lib.Exceptions;

namespace RedResQ_API.Lib.Services
{
    public static class LanguageService
    {
        public static Language[] GetAll()
        {
            List<Language> languages = new List<Language>();
            string storedProcedure = "SP_La_GetAllLanguages";

            DataTable languageTable = SqlHandler.ExecuteQuery(storedProcedure);

            if (languageTable.Rows.Count > 0)
            {
                foreach (DataRow row in languageTable.Rows)
                {
                    languages.Add(Converter.ToLanguage(row.ItemArray.ToList()!));
                }

                return languages.ToArray();
            }

            throw new NotFoundException("No Languages were found!");
        }

        public static Language Get(long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_La_GetLanguage";

            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

            DataTable countryTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (countryTable.Rows.Count == 1)
            {
                return Converter.ToLanguage(countryTable.Rows[0].ItemArray.ToList()!);
            }

            throw new NotFoundException("No Languages were found!");
        }

        public static bool Add(string name)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_La_AddLanguage";

            parameters.Add(new SqlParameter { ParameterName = "@languageName", SqlDbType = SqlDbType.VarChar, Value = name });

            int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

            if (rowsAffected == 1)
            {
                return true;
            }

            return false;
        }

        public static bool Edit(Language lang)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_La_EditProcedure";

            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = lang.Id });
            parameters.Add(new SqlParameter { ParameterName = "@languageName", SqlDbType = SqlDbType.VarChar, Value = lang.Name });

            int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

            if (rowsAffected == 1)
            {
                return true;
            }

            return false;
        }

        public static bool Delete(long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_La_DeleteProcedure";

            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

            int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

            if (rowsAffected == 1)
            {
                return true;
            }

            return false;
        }
    }
}
