using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class NotificationController : ControllerBase
    {
        [HttpGet("test")]
        public ActionResult<string> SendNotification(string token, string title, string desc)
        {
            return ActionService.Execute(this, () =>
            {
                return Ok(NotificationService.SendNotification(token, title, desc).Result);
            });
        }
    }
}
