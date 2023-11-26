using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedResQ_API.Lib;
using RedResQ_API.Lib.Models;
using RedResQ_API.Lib.Services;
using System.Data;
using System.Text.Json.Serialization;

namespace RedResQ_API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class NewsController : ControllerBase
	{
		[HttpGet]
		[Authorize]
		public ActionResult<Article[]> GetArticles(long? articleId, long? countryId, long? languageId)
		{
			Article[] articles = null!;

			try
			{
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

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost]
		[Authorize]
		public ActionResult AddArticle(RawArticle article)
		{
			JwtClaims claims = JwtHandler.GetClaims(this);

			try
			{
				int rowsaffected = NewsService.AddArticle(claims, article);

				return Ok($"Article was successfully added. Number of rows affected: {rowsaffected}");
			}
			catch (DataException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return Forbid(ex.Message);
			}
		}
	}
}
