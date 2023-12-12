using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]")]
    public class LanguageController : ControllerBase
    {
        [HttpGet("fetch")]
        public ActionResult<Language[]> GetAll()
        {
            try
            {
                return Ok(LanguageService.GetAll());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get")]
        public ActionResult<Language> Get(long id)
        {
            try
            {
                return Ok(LanguageService.Get(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add"), Authorize]
        public ActionResult<bool> Add(string name)
        {
            try
            {
                return Ok(LanguageService.Add(JwtHandler.GetClaims(this), name));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update"), Authorize]
        public ActionResult<bool> Edit(Language lang)
        {
            try
            {
                return Ok(LanguageService.Edit(JwtHandler.GetClaims(this), lang));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete"), Authorize]
        public ActionResult<bool> Delete(long id)
        {
            try
            {
                return Ok(LanguageService.Delete(JwtHandler.GetClaims(this), id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
