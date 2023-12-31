﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedResQ_API.Lib.Models;
using System.Security.Claims;
using System.Net;

namespace RedResQ_API.Lib.Services
{
    public static class UserService
    {
        /// <summary>
        /// Returns an array of user objects from the database
        /// </summary>
        /// <param name="claims">The security claims from the JWT</param>
        /// <param name="id">Identifier of the first object to be excluded, to fetch more users after this one</param>
        /// <param name="amount">Number of users to fetch in one run</param>
        /// <returns>The array of user objects</returns>
        /// <exception cref="Exception">Is thrown when no user objects were found</exception>
        public static User[] Fetch(JwtClaims claims, long? id, int? amount)
        {
            if (PermissionService.IsPermitted("fetchUsers", claims.Role))
            {
                List<User> users = new List<User>();
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Pe_FetchUsers";

                if(id.HasValue)
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
                        users.Add(User.ConvertToPerson(row));
                    }

                    return users.ToArray();
                }
            }

            throw new Exception("No Users were found!");
        }

        /// <summary>
        /// Searches for a user in the database by a given string
        /// </summary>
        /// <param name="claims">The security claims from the JWT</param>
        /// <param name="query">The string to search for</param>
        /// <returns>An array of user datasets matching the string</returns>
        /// <exception cref="Exception">Is thrown when no users matching the query were found</exception>
        public static User[] Search(JwtClaims claims, string query)
        {
            if (PermissionService.IsPermitted("searchForUser", claims.Role))
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
                        users.Add(User.ConvertToPerson(row));
                    }

                    return users.ToArray();
                }
            }

            throw new Exception("No Users were found!");
        }

        /// <summary>
        /// Grabs a User object from the database by its ID
        /// </summary>
        /// <param name="claims">The security claims from the JWT</param>
        /// <param name="id">Identifier of the object to get from the database</param>
        /// <returns>The targeted user object</returns>
        /// <exception cref="Exception">Is thrown when no user with the given id was found</exception>
        public static User GetAny(JwtClaims claims, long id)
        {
            if (PermissionService.IsPermitted("getAnyUser", claims.Role))
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Pe_GetUser";

                parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.VarChar, Value = id });

                DataTable userTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

                if (userTable.Rows.Count == 1)
                {
                    return User.ConvertToPerson(userTable.Rows[0]);
                }
            }

            throw new Exception("No User was found!");
        }

        /// <summary>
        /// Gets a user object based on the username stored inside of the JWT
        /// </summary>
        /// <param name="claims">The security claims from the JWT</param>
        /// <returns>The user object linked to the JWT</returns>
        /// <exception cref="Exception">Is thrown when there is no user linked to the username inside of the JWT</exception>
        public static User GetPersonal(JwtClaims claims)
        {
            if (PermissionService.IsPermitted("getPersonalUser", claims.Role))
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Pe_GetUser";

                parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.VarChar, Value = GetID(claims.Username) });

                DataTable userTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

                if (userTable.Rows.Count == 1)
                {
                    return User.ConvertToPerson(userTable.Rows[0]);
                }
            }

            throw new Exception("No User was found!");
        }

        /// <summary>
        /// Checks if a user in the database already uses the given username
        /// </summary>
        /// <param name="claims">The security claims from the JWT</param>
        /// <param name="username">The username to check</param>
        /// <returns>true if the username does not exist in the database, false if it does</returns>
        public static bool CheckUsername(JwtClaims claims, string username)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Pe_CheckUsername";

            parameters.Add(new SqlParameter { ParameterName = "@username", SqlDbType = SqlDbType.VarChar, Value = username });

            return CheckExistence(claims, storedProcedure, parameters.ToArray());
        }

        /// <summary>
        /// Checks if a user in the database already uses the given email
        /// </summary>
        /// <param name="claims">SP_Pe_CheckEmailThe security claims from the JWT</param>
        /// <param name="email">The email to check</param>
        /// <returns>true if the email does not exist in the database, false if it does</returns>
        public static bool CheckEmail(JwtClaims claims, string email)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Pe_CheckEmail";

            parameters.Add(new SqlParameter { ParameterName = "@email", SqlDbType = SqlDbType.VarChar, Value = email });

            return CheckExistence(claims, storedProcedure, parameters.ToArray());
        }

        /// <summary>
        /// Compares a given user object to its saved data in the database and replaces different values based on the given data
        /// </summary>
        /// <param name="claims">The security claims from the JWT</param>
        /// <param name="user">The given user object possibly containing new data</param>
        /// <returns>true, if the update was successful, false if not</returns>
        public static bool Edit(JwtClaims claims, User user)
        {
            if (PermissionService.IsPermitted("editUser", claims.Role))
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Pe_EditUser";

                User oldUser = GetAny(claims, user.Id);

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

            return false;
        }

        /// <summary>
        /// Sets the role of a user
        /// </summary>
        /// <param name="claims">The security claims from the JWT</param>
        /// <param name="userId">The identifier of the targeted user</param>
        /// <param name="roleId">The identifier of the role to set for the user</param>
        /// <returns>true if the role was successfully set, false if not</returns>
        public static bool Promote(JwtClaims claims, long userId, long roleId)
        {
            Role role = RoleService.Get(claims, roleId);

            if(PermissionService.IsPermitted("promoteTo" + role.Name.Replace(" ", ""), claims.Role) && role != null)
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
            }

            return false;
        }

        /// <summary>
        /// Deletes a user from the database based on a given identifier
        /// </summary>
        /// <param name="claims">The security claims from the JWT</param>
        /// <param name="id">The identifier needed to delete the row from the database</param>
        /// <returns>true if the row was successfully deleted, false if not</returns>
        public static bool Delete(JwtClaims claims, long id)
        {
            if (PermissionService.IsPermitted("deleteUser", claims.Role))
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

            return false;
        }

        /// <summary>
        /// Gets the ID of a user object from the database based on its unique username
        /// </summary>
        /// <param name="username">The username of the targeted user</param>
        /// <returns>The identifier of the target user object</returns>
        /// <exception cref="Exception">Is thrown when there is no user with the given username</exception>
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

            throw new Exception("No User was found!");
        }

        /// <summary>
        /// Checks if an Email or a username already exists in the User table of the database
        /// </summary>
        /// <param name="claims">The security claims from the JWT</param>
        /// <param name="storedProcedure">The name of the stored procedure to execute</param>
        /// <param name="parameters">All SqlParameters needed for the stored procedure</param>
        /// <returns>true if the username or email does not exist, false if it does</returns>
        /// <exception cref="Exception">Is thrown when the user is not permitted to check the existence of an email or a username</exception>
        private static bool CheckExistence(JwtClaims claims, string storedProcedure, SqlParameter[] parameters)
        {
            if(PermissionService.IsPermitted("checkExistenceOfEmailOrUsername", claims.Role))
            {
                DataTable userTable = SqlHandler.ExecuteQuery(storedProcedure, parameters);

                if(userTable.Rows.Count > 0)
                {
                    return false;
                }

                return true;
            }

            throw new Exception("Not permitted");
        }
    }
}
 