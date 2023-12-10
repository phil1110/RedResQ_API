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
                    languages.Add(Language.ConvertToLanguage(row));
                }

                return languages.ToArray();
            }

            throw new Exception("No Languages were found!");
        }

        public static Language Get(long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_La_GetLanguage";

            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

            DataTable countryTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (countryTable.Rows.Count == 1)
            {
                return Language.ConvertToLanguage(countryTable.Rows[0]);
            }

            throw new Exception("Row count was not 1!");
        }

        public static bool Add(JwtClaims claims, string name)
        {
            if (PermissionService.IsPermitted("addLanguage", claims.Role))
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

            throw new Exception("Not permitted to complete this action!");
        }

        public static bool Edit(JwtClaims claims, Language lang)
        {
            if (PermissionService.IsPermitted("editLanguage", claims.Role))
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

            throw new Exception("Not permitted to complete this action!");
        }

        public static bool Delete(JwtClaims claims, long id)
        {
            if (PermissionService.IsPermitted("deleteLanguage", claims.Role))
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

            throw new Exception("Not permitted to complete this action!");
        }
    }
}
