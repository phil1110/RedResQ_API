using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class NotificationController : ControllerBase
    {
        [HttpGet("test")]
        public ActionResult<string> SendNotification(string token, string title, string desc)
        {
            return ActionService.Execute(this, "sendNotification", () =>
            {
                return Ok(NotificationService.SendNotification(token, title, desc).Result);
            });
        }

        [HttpGet("hazard")]
        public ActionResult<string> SendHazardNotification(long hazardId, string title, string desc)
        {
            return ActionService.Execute(this, "sendNotification", () =>
            {
                return Ok(NotificationService.SendHazardNotification(hazardId, title, desc).Result);
            });
        }
    }
}
