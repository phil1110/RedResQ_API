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
            return ActionService.Execute(this, () =>
            {
                Article[] articles = null!;

                if (countryId.HasValue)
                {

                    if (languageId.HasValue)
                    {
                        articles = NewsService.GetCountryAndLanguageArticles(JwtHandler.GetClaims(this), countryId.Value, languageId.Value, articleId);
                    }
                    else
                    {
                        articles = NewsService.GetCountryArticles(JwtHandler.GetClaims(this), countryId.Value, articleId);
                    }

                }
                else
                {

                    if (languageId.HasValue)
                    {
                        articles = NewsService.GetLanguageArticles(JwtHandler.GetClaims(this), languageId.Value, articleId);
                    }
                    else
                    {
                        articles = NewsService.GetGlobalArticles(JwtHandler.GetClaims(this), articleId);
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
            return ActionService.Execute(this, () =>
            {
                Article article = NewsService.GetSingleArticle(JwtHandler.GetClaims(this), id);

                return Ok(article);
            });
		}

		[HttpPost("add")]
		public ActionResult AddArticle(RawArticle article)
		{
			JwtClaims claims = JwtHandler.GetClaims(this);

            return ActionService.Execute(this, () =>
            {
                int rowsaffected = NewsService.AddArticle(claims, article);

                return Ok($"Article was successfully added. Number of rows affected: {rowsaffected}");
            });
		}

		[HttpPut("update")]
		public ActionResult EditArticle(Article article)
		{
            return ActionService.Execute(this, () =>
            {
                bool articleEdited = NewsService.UpdateArticle(JwtHandler.GetClaims(this), article);

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
            return ActionService.Execute(this, () =>
            {
                bool articleDeleted = NewsService.DeleteArticle(JwtHandler.GetClaims(this), articleId);

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
