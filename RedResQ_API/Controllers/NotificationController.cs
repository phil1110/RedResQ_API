using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class NotificationController : ControllerBase
    {
        [HttpPost("hazard")]
        public ActionResult<string> SendHazardNotification(long hazardId, string title, string desc)
        {
            return ActionService.Execute(this, "sendNotification", () =>
            {
                return Ok(NotificationService.SendHazardNotification(hazardId, title, desc).Result);
            });
        }
    }
}
