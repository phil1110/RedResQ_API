using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using RedResQ_API.Lib.Models;
using System.Data.SqlClient;
using System.Data;
using System.Net;

namespace RedResQ_API.Lib.Services
{
	public class NewsService
	{
		#region Get - Methods

		public static Article[] GetGlobalArticles(JwtClaims claims, long? articleId)
		{
			if(PermissionService.IsPermitted("", claims.Role))
			{
                List<Article> output = new List<Article>();
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Ne_LatestArticles_Global";

                if (articleId.HasValue)
                {
                    parameters.Add(new SqlParameter { ParameterName = "@articleId", SqlDbType = SqlDbType.BigInt, Value = articleId.Value });
                }

                DataTable articles = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

                if (articles.Rows.Count > 0)
                {
                    foreach (DataRow row in articles.Rows)
                    {
                        output.Add(Article.ConvertToArticle(row));
                    }

                    return output.ToArray();
                }
                else
                {
                    throw new Exception("No Articles were found!");
                }
            }

			throw new Exception("Not Authorized!");
		}

		public static Article[] GetCountryArticles(JwtClaims claims, long countryId, long? articleId)
		{
            if (PermissionService.IsPermitted("", claims.Role))
            {
                List<Article> output = new List<Article>();
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Ne_LatestArticles_Country";

                parameters.Add(new SqlParameter { ParameterName = "@countryId", SqlDbType = SqlDbType.BigInt, Value = countryId });

                if (articleId.HasValue)
                {
                    parameters.Add(new SqlParameter { ParameterName = "@articleId", SqlDbType = SqlDbType.BigInt, Value = articleId.Value });
                }

                DataTable articles = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

                if (articles.Rows.Count > 0)
                {
                    foreach (DataRow row in articles.Rows)
                    {
                        output.Add(Article.ConvertToArticle(row));
                    }

                    return output.ToArray();
                }
                else
                {
                    throw new Exception("No Articles were found!");
                }
            }

            throw new Exception("Not Authorized!");
		}

		public static Article[] GetLanguageArticles(JwtClaims claims, long languageId, long? articleId)
		{
            if (PermissionService.IsPermitted("", claims.Role))
            {
                List<Article> output = new List<Article>();
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Ne_LatestArticles_Language";

                parameters.Add(new SqlParameter { ParameterName = "@languageId", SqlDbType = SqlDbType.BigInt, Value = languageId });

                if (articleId.HasValue)
                {
                    parameters.Add(new SqlParameter { ParameterName = "@articleId", SqlDbType = SqlDbType.BigInt, Value = articleId.Value });
                }

                DataTable articles = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

                if (articles.Rows.Count > 0)
                {
                    foreach (DataRow row in articles.Rows)
                    {
                        output.Add(Article.ConvertToArticle(row));
                    }

                    return output.ToArray();
                }
                else
                {
                    throw new Exception("No Articles were found!");
                }
            }

            throw new Exception("Not Authorized!");
		}

		public static Article[] GetCountryAndLanguageArticles(JwtClaims claims, long countryId, long languageId, long? articleId)
		{
            if (PermissionService.IsPermitted("", claims.Role))
            {
                List<Article> output = new List<Article>();
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Ne_LatestArticles_CountryAndLanguage";

                parameters.Add(new SqlParameter { ParameterName = "@countryId", SqlDbType = SqlDbType.BigInt, Value = countryId });
                parameters.Add(new SqlParameter { ParameterName = "@languageId", SqlDbType = SqlDbType.BigInt, Value = languageId });

                if (articleId.HasValue)
                {
                    parameters.Add(new SqlParameter { ParameterName = "@articleId", SqlDbType = SqlDbType.BigInt, Value = articleId.Value });
                }

                DataTable articles = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

                if (articles.Rows.Count > 0)
                {
                    foreach (DataRow row in articles.Rows)
                    {
                        output.Add(Article.ConvertToArticle(row));
                    }

                    return output.ToArray();
                }
                else
                {
                    throw new Exception("No Articles were found!");
                }
            }

            throw new Exception("Not Authorized!");
		}

		public static Article GetSingleArticle(JwtClaims claims, long articleId)
		{
            if (PermissionService.IsPermitted("", claims.Role))
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                string storedProcedure = "SP_Ne_SpecificArticle";

                parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = articleId });

                DataTable articleTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

                if (articleTable.Rows.Count == 1)
                {
                    return Article.ConvertToArticle(articleTable.Rows[0]);
                }

                throw new NullReferenceException("This article does not exist in the database!");
            }

            throw new Exception("Not Authorized!");
		}

		#endregion

		#region Post - Methods

		public static int AddArticle(JwtClaims claims, RawArticle article)
		{
			if(PermissionService.IsPermitted("publishArticle", claims.Role))
			{
				List<SqlParameter> parameters = new List<SqlParameter>();
				string storedProcedure = "SP_Ne_NewArticle";

				parameters.Add(new SqlParameter { ParameterName = "@title", SqlDbType = SqlDbType.VarChar, Value = article.Title });
				parameters.Add(new SqlParameter { ParameterName = "@content", SqlDbType = SqlDbType.VarChar, Value = article.Content });
				parameters.Add(new SqlParameter { ParameterName = "@author", SqlDbType = SqlDbType.VarChar, Value = article.Author });
				parameters.Add(new SqlParameter { ParameterName = "@date", SqlDbType = SqlDbType.DateTime, Value = article.Date });
				parameters.Add(new SqlParameter { ParameterName = "@languageId", SqlDbType = SqlDbType.BigInt, Value = article.Language });
				parameters.Add(new SqlParameter { ParameterName = "@imageId", SqlDbType = SqlDbType.BigInt, Value = article.Image });
				parameters.Add(new SqlParameter { ParameterName = "@locationId", SqlDbType = SqlDbType.BigInt, Value = article.Location });

				int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

				if(rowsAffected > 0)
				{
					return rowsAffected;
				}
				else
				{
					throw new DataException("Error while adding new Article!");
				}
			}
			else
			{
				throw new Exception("Not authorized to complete this Action!");
			}

			throw new NotImplementedException();
		}

		#endregion

		#region Put - Methods

		public static bool UpdateArticle(JwtClaims claims, Article article)
		{
			if(PermissionService.IsPermitted("editArticle", claims.Role))
			{
				string storedProcedure = "SP_Ne_UpdateArticle";
				List<SqlParameter> parameters = new List<SqlParameter>();
				Article oldArticle = GetSingleArticle(claims, article.Id);

				parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = article.Id });

				if (article.Title != oldArticle.Title)
				{
					parameters.Add(new SqlParameter { ParameterName = "@title", SqlDbType = SqlDbType.VarChar, Value = article.Title });
				}

				if (article.Content != oldArticle.Content)
				{
					parameters.Add(new SqlParameter { ParameterName = "@content", SqlDbType = SqlDbType.VarChar, Value = article.Content });
				}

				if (parameters.Count > 1)
				{
					int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

					if (rowsAffected > 0)
					{
						return true;
					}
					else
					{
						return false;
					}
				}

				return false;
			}

			throw new Exception("Not authorized to complete this Action!");
		}

		#endregion

		#region Delete - Methods

		public static bool DeleteArticle(JwtClaims claims, long articleId)
		{
			if (PermissionService.IsPermitted("deleteArticle", claims.Role))
			{
				string storedProcedure = "SP_Ne_DeleteArticle";
				List<SqlParameter> parameters = new List<SqlParameter>();

				parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = articleId });

				int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

				if(rowsAffected > 0)
				{
					return true;
				}

				return false;
			}

			throw new Exception("Not authorized to complete this Action!");
		}

		#endregion
	}
}
