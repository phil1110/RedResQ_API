using Microsoft.AspNetCore.Mvc;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]")]
    public class GuestController : ControllerBase
    {
        [HttpGet("request")]
        public ActionResult GetToken()
        {
            return ActionService.Execute(this, () =>
            {
                return Ok(JwtHandler.CreateGuestToken(this));
            });
        }
    }
}
