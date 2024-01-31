using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedResQ_API.Lib.Models;
using System.Security.Claims;
using System.Net;
using RedResQ_API.Lib.Exceptions;

namespace RedResQ_API.Lib.Services
{
    public static class UserService
    {
        public static User[] Fetch(long? id, int? amount)
        {
            List<User> users = new List<User>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Pe_FetchUsers";

            if (id.HasValue)
            {
                parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id.Value });
            }
            if (amount.HasValue)
            {
                parameters.Add(new SqlParameter { ParameterName = "@amount", SqlDbType = SqlDbType.Int, Value = amount.Value });
            }

            DataTable userTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (userTable.Rows.Count > 0)
            {
                foreach (DataRow row in userTable.Rows)
                {
                    users.Add(Converter.ToUser(row.ItemArray.ToList()!));
                }

                return users.ToArray();
            }

            throw new NotFoundException("No Users were found!");
        }

        public static User[] Search(string query)
        {
            List<User> users = new List<User>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Pe_SearchForUser";

            parameters.Add(new SqlParameter { ParameterName = "@searchString", SqlDbType = SqlDbType.VarChar, Value = query });

            DataTable userTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (userTable.Rows.Count > 0)
            {
                foreach (DataRow row in userTable.Rows)
                {
                    users.Add(Converter.ToUser(row.ItemArray.ToList()!));
                }

                return users.ToArray();
            }

            throw new NotFoundException("No Users were found!");
        }

        public static User GetSpecific(long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Pe_GetUser";

            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.VarChar, Value = id });

            DataTable userTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (userTable.Rows.Count == 1)
            {
                return Converter.ToUser(userTable.Rows[0].ItemArray.ToList()!);
            }

            throw new NotFoundException("No User was found!");
        }

        public static User GetPersonal(JwtClaims claims)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Pe_GetUser";

            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.VarChar, Value = GetID(claims.Username) });

            DataTable userTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (userTable.Rows.Count == 1)
            {
                return Converter.ToUser(userTable.Rows[0].ItemArray.ToList()!);
            }

            throw new Exception("No User was found!");
        }

        public static bool CheckUsername(JwtClaims claims, string username)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Pe_CheckUsername";

            parameters.Add(new SqlParameter { ParameterName = "@username", SqlDbType = SqlDbType.VarChar, Value = username });

            return CheckExistence(claims, storedProcedure, parameters.ToArray());
        }

        public static bool CheckEmail(JwtClaims claims, string email)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Pe_CheckEmail";

            parameters.Add(new SqlParameter { ParameterName = "@email", SqlDbType = SqlDbType.VarChar, Value = email });

            return CheckExistence(claims, storedProcedure, parameters.ToArray());
        }

        public static bool Edit(User user)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Pe_EditUser";

            User oldUser = GetSpecific(user.Id);

            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = user.Id });

            if (user.Username != oldUser.Username)
            {
                parameters.Add(new SqlParameter { ParameterName = "@username", SqlDbType = SqlDbType.VarChar, Value = user.Username });
            }
            if (user.FirstName != oldUser.FirstName)
            {
                parameters.Add(new SqlParameter { ParameterName = "@firstname", SqlDbType = SqlDbType.VarChar, Value = user.FirstName });
            }
            if (user.LastName != oldUser.LastName)
            {
                parameters.Add(new SqlParameter { ParameterName = "@lastname", SqlDbType = SqlDbType.VarChar, Value = user.LastName });
            }
            if (user.Email != oldUser.Email)
            {
                parameters.Add(new SqlParameter { ParameterName = "@email", SqlDbType = SqlDbType.VarChar, Value = user.Email });
            }
            if (user.Birthdate != oldUser.Birthdate)
            {
                parameters.Add(new SqlParameter { ParameterName = "@birthdate", SqlDbType = SqlDbType.DateTime, Value = user.Birthdate });
            }
            if (user.Gender.Id != oldUser.Gender.Id)
            {
                parameters.Add(new SqlParameter { ParameterName = "@genderId", SqlDbType = SqlDbType.BigInt, Value = user.Gender.Id });
            }
            if (user.Language.Id != oldUser.Language.Id)
            {
                parameters.Add(new SqlParameter { ParameterName = "@languageId", SqlDbType = SqlDbType.BigInt, Value = user.Language.Id });
            }
            if (user.Location.Id != oldUser.Location.Id)
            {
                parameters.Add(new SqlParameter { ParameterName = "@locationId", SqlDbType = SqlDbType.BigInt, Value = user.Location.Id });
            }

            int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

            if (rowsAffected > 0)
            {
                return true;
            }

            return false;
        }

        public static bool Promote(long userId, Role role)
        {

            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Pe_SetRole";

            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = userId });
            parameters.Add(new SqlParameter { ParameterName = "@roleId", SqlDbType = SqlDbType.BigInt, Value = role.Id });

            int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

            if (rowsAffected > 0)
            {
                return true;
            }

            return false;
        }

        public static bool Delete(long id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Pe_DeleteUser";

            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = id });

            int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

            if (rowsAffected > 0)
            {
                return true;
            }

            return false;
        }

        private static long GetID(string username)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Pe_GetID";

            parameters.Add(new SqlParameter { ParameterName = "@username", SqlDbType = SqlDbType.VarChar, Value = username });

            DataTable userTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (userTable.Rows.Count == 1)
            {
                return (long)userTable.Rows[0].ItemArray[0]!;
            }

            throw new NotFoundException("No User was found!");
        }

        private static bool CheckExistence(JwtClaims claims, string storedProcedure, SqlParameter[] parameters)
        {
            DataTable userTable = SqlHandler.ExecuteQuery(storedProcedure, parameters);

            if (userTable.Rows.Count > 0)
            {
                return false;
            }

            return true;
        }
    }
}
 