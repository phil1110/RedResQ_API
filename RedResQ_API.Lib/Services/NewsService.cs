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

		public static Article[] GetGlobalArticles(long? articleId)
		{
			List<Article> output = new List<Article>();
			List<SqlParameter> parameters = new List<SqlParameter>();
			string storedProcedure = "SP_Ne_LatestArticles_Global";

			if (articleId.HasValue)
			{
				parameters.Add(new SqlParameter { ParameterName = "@articleId", SqlDbType = SqlDbType.BigInt, Value = articleId.Value });
			}

			DataTable articles = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

			if(articles.Rows.Count > 0)
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

		public static Article[] GetCountryArticles(long countryId, long? articleId)
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

		public static Article[] GetLanguageArticles(long languageId, long? articleId)
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

		public static Article[] GetCountryAndLanguageArticles(long countryId, long languageId, long? articleId)
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

		#endregion

		#region Add - Methods

		public static int AddArticle(JwtClaims claims, RawArticle article)
		{
			if(claims.Role > 3)
			{
				List<SqlParameter> parameters = new List<SqlParameter>();
				string storedProcedure = "SP_Ne_NewArticle";

				parameters.Add(new SqlParameter { ParameterName = "@title", SqlDbType = SqlDbType.VarChar, Value = article.Title });
				parameters.Add(new SqlParameter { ParameterName = "@content", SqlDbType = SqlDbType.BigInt, Value = article.Content });
				parameters.Add(new SqlParameter { ParameterName = "@author", SqlDbType = SqlDbType.BigInt, Value = article.Author });
				parameters.Add(new SqlParameter { ParameterName = "@date", SqlDbType = SqlDbType.DateTime, Value = article.Date });
				parameters.Add(new SqlParameter { ParameterName = "@languageId", SqlDbType = SqlDbType.BigInt, Value = article.Language.Id });
				parameters.Add(new SqlParameter { ParameterName = "@imageId", SqlDbType = SqlDbType.BigInt, Value = article.Image.Id });
				parameters.Add(new SqlParameter { ParameterName = "@locationId", SqlDbType = SqlDbType.BigInt, Value = article.Location.Id });

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
	}
}
