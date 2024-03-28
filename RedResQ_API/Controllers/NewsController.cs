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
		public ActionResult<Article[]> FetchArticles(long? articleId, long? countryId, long? languageId, string? query)
		{
            return ActionService.Execute(this, "getArticle", () =>
            {
                return Ok(NewsService.FetchArticles(articleId, countryId, languageId, query));
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
