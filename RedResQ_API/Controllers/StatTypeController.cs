using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class StatTypeController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string[]> Fetch()
        {
            return ActionService.Execute(this, "getStatType", () => {
                return Ok(StatTypeService.Fetch());
            });
        }
    }
}
