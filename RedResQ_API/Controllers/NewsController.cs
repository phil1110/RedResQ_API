using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text.Json.Serialization;

namespace RedResQ_API.Controllers
{
	[ApiController, Route("[controller]"), Authorize]
	public class NewsController : ControllerBase
	{
		[HttpGet("fetch")]
		public ActionResult<Article[]> GetArticles(long? articleId, long? countryId, long? languageId)
		{
            return ActionService.Execute(this, "getArticle", () =>
            {
                Article[] articles = null!;

                if (countryId.HasValue)
                {

                    if (languageId.HasValue)
                    {
                        articles = NewsService.GetCountryAndLanguageArticles(countryId.Value, languageId.Value, articleId);
                    }
                    else
                    {
                        articles = NewsService.GetCountryArticles(countryId.Value, articleId);
                    }

                }
                else
                {

                    if (languageId.HasValue)
                    {
                        articles = NewsService.GetLanguageArticles(languageId.Value, articleId);
                    }
                    else
                    {
                        articles = NewsService.GetGlobalArticles(articleId);
                    }

                }

                if (articles != null)
                {
                    return Ok(articles);
                }
                else { return BadRequest(); }
            });
		}

		[HttpGet("get")]
		public ActionResult<Article> GetArticle(long id)
		{
            return ActionService.Execute(this, "getArticle", () =>
            {
                Article article = NewsService.GetSingleArticle(id);

                return Ok(article);
            });
		}

		[HttpPost("add")]
		public ActionResult AddArticle(RawArticle article)
		{
			JwtClaims claims = JwtHandler.GetClaims(this);

            return ActionService.Execute(this, "publishArticle", () =>
            {
                int rowsaffected = NewsService.AddArticle(article);

                return Ok($"Article was successfully added. Number of rows affected: {rowsaffected}");
            });
		}

		[HttpPut("update")]
		public ActionResult EditArticle(Article article)
		{
            return ActionService.Execute(this, "editArticle", () =>
            {
                bool articleEdited = NewsService.UpdateArticle(article);

                if (articleEdited)
                {
                    return Ok($"Article (with ID {article.Id} ) was successfully edited!");
                }
                else
                {
                    return BadRequest("Article was not edited!");
                }
            });
		}

		[HttpDelete("delete")]
		public ActionResult RemoveArticle(long articleId)
		{
            return ActionService.Execute(this, "deleteArticle", () =>
            {
                bool articleDeleted = NewsService.DeleteArticle(articleId);

                if (articleDeleted)
                {
                    return Ok($"Article (with ID {articleId} ) was successfully deleted!");
                }
                else
                {
                    return BadRequest("Article was not deleted!");
                }
            });
		}
	}
}
