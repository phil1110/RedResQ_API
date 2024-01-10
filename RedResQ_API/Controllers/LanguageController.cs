using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using RedResQ_API.Lib.Exceptions;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class LanguageController : ControllerBase
    {
        [HttpGet("fetch")]
        public ActionResult<Language[]> GetAll()
        {
            return ActionService.Execute(this, () =>
            {
                return Ok(LanguageService.GetAll(JwtHandler.GetClaims(this)));
            });
        }

        [HttpGet("get")]
        public ActionResult<Language> Get(long id)
        {
            return ActionService.Execute(this, () =>
            {
                return Ok(LanguageService.Get(JwtHandler.GetClaims(this), id));
            });
        }

        [HttpPost("add")]
        public ActionResult<bool> Add(string name)
        {
            return ActionService.Execute(this, () =>
            {
                return Ok(LanguageService.Add(JwtHandler.GetClaims(this), name));
            });
        }

        [HttpPut("update")]
        public ActionResult<bool> Edit(Language lang)
        {
            return ActionService.Execute(this, () =>
            {
                return Ok(LanguageService.Edit(JwtHandler.GetClaims(this), lang));
            });
        }

        [HttpDelete("delete")]
        public ActionResult<bool> Delete(long id)
        {
            return ActionService.Execute(this, () =>
            {
                return Ok(LanguageService.Delete(JwtHandler.GetClaims(this), id));
            });
        }
    }
}
