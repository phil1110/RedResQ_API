using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class LanguageController : ControllerBase
    {
        [HttpGet("fetch")]
        public ActionResult<Language[]> GetAll()
        {
            return ActionService.Execute(this, "getLanguage", () =>
            {
                return Ok(LanguageService.GetAll());
            });
        }

        [HttpGet("get")]
        public ActionResult<Language> Get(long id)
        {
            return ActionService.Execute(this, "getLanguage", () =>
            {
                return Ok(LanguageService.Get(id));
            });
        }

        [HttpPost("add")]
        public ActionResult<bool> Add(string name)
        {
            return ActionService.Execute(this, "addLanguage", () =>
            {
                return Ok(LanguageService.Add(name));
            });
        }

        [HttpPut("update")]
        public ActionResult<bool> Edit(Language lang)
        {
            return ActionService.Execute(this, "editLanguage", () =>
            {
                return Ok(LanguageService.Edit(lang));
            });
        }

        [HttpDelete("delete")]
        public ActionResult<bool> Delete(long id)
        {
            return ActionService.Execute(this, "deleteLanguage", () =>
            {
                return Ok(LanguageService.Delete(id));
            });
        }
    }
}
