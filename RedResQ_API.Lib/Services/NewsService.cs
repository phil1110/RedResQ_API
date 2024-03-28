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
using RedResQ_API.Lib.Exceptions;

namespace RedResQ_API.Lib.Services
{
	public class NewsService
	{
		#region Get - Methods

		public static Article[] FetchArticles(long? articleId, long? countryId, long? languageId, string? query)
		{
            List<Article> articles = new List<Article>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Ar_FetchArticles";

            if (articleId.HasValue)
            {
                parameters.Add(new SqlParameter { ParameterName = "@articleId", SqlDbType = SqlDbType.BigInt, Value = articleId });
            }
            if(countryId.HasValue)
            {
                parameters.Add(new SqlParameter { ParameterName = "@countryId", SqlDbType = SqlDbType.BigInt, Value = countryId });
            }
            if (languageId.HasValue)
            {
                parameters.Add(new SqlParameter { ParameterName = "@languageId", SqlDbType = SqlDbType.BigInt, Value = languageId });
            }
            if (query != null)
            {
                parameters.Add(new SqlParameter { ParameterName = "@query", SqlDbType = SqlDbType.VarChar, Value = query });
            }

            DataTable articleTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (articleTable.Rows.Count > 0)
            {
                foreach (DataRow row in articleTable.Rows)
                {
                    articles.Add(Converter.ToArticle(row.ItemArray.ToList()!));
                }

                return articles.ToArray();
            }
            else
            {
                throw new NotFoundException("No Articles were found!");
            }
        }
        

		public static Article GetSingleArticle(long articleId)
		{
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Ar_SpecificArticle";

            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = articleId });

            DataTable articleTable = SqlHandler.ExecuteQuery(storedProcedure, parameters.ToArray());

            if (articleTable.Rows.Count == 1)
            {
                return Converter.ToArticle(articleTable.Rows[0].ItemArray.ToList()!);
            }

            throw new NotFoundException("This article does not exist in the database!");
        }

		#endregion

		#region Post - Methods

		public static int AddArticle(RawArticle article)
		{
            List<SqlParameter> parameters = new List<SqlParameter>();
            string storedProcedure = "SP_Ar_NewArticle";

            parameters.Add(new SqlParameter { ParameterName = "@title", SqlDbType = SqlDbType.VarChar, Value = article.Title });
            parameters.Add(new SqlParameter { ParameterName = "@content", SqlDbType = SqlDbType.VarChar, Value = article.Content });
            parameters.Add(new SqlParameter { ParameterName = "@author", SqlDbType = SqlDbType.VarChar, Value = article.Author });
            parameters.Add(new SqlParameter { ParameterName = "@date", SqlDbType = SqlDbType.DateTime, Value = article.Date });
            parameters.Add(new SqlParameter { ParameterName = "@languageId", SqlDbType = SqlDbType.BigInt, Value = article.Language });
            parameters.Add(new SqlParameter { ParameterName = "@imageId", SqlDbType = SqlDbType.BigInt, Value = article.Image });
            parameters.Add(new SqlParameter { ParameterName = "@countryId", SqlDbType = SqlDbType.BigInt, Value = article.Country });

            int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

            if (rowsAffected > 0)
            {
                return rowsAffected;
            }
            else
            {
                throw new UnprocessableEntityException("Error while adding new Article!");
            }
        }

		#endregion

		#region Put - Methods

		public static bool UpdateArticle(Article article)
		{
            string storedProcedure = "SP_Ar_UpdateArticle";
            List<SqlParameter> parameters = new List<SqlParameter>();
            Article oldArticle = GetSingleArticle(article.Id);

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

		#endregion

		#region Delete - Methods

		public static bool DeleteArticle(long articleId)
		{
            string storedProcedure = "SP_Ar_DeleteArticle";
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter { ParameterName = "@id", SqlDbType = SqlDbType.BigInt, Value = articleId });

            int rowsAffected = SqlHandler.ExecuteNonQuery(storedProcedure, parameters.ToArray());

            if (rowsAffected > 0)
            {
                return true;
            }

            return false;
        }

		#endregion
	}
}
