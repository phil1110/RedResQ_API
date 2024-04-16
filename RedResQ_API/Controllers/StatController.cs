using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class StatController : ControllerBase
    {
        [HttpGet]
        public ActionResult<Dictionary<string, int>> GetStat(string statName)
        {
            return ActionService.Execute(this, "getStat", () =>
            {
                return Ok(StatService.GetStat(statName));
            });
        }
    }
}
