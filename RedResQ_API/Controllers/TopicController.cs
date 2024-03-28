using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedResQ_API.Lib.Models;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class TopicController : ControllerBase
    {
        [HttpGet("active")]
        public ActionResult<string[]> GetActiveTopics(float lat, float lon)
        {
            return ActionService.Execute(this, "getTopics", () =>
            {
                return Ok(TopicService.GetTopics(lat, lon));
            });
        }
    }
}
