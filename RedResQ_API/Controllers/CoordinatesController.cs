using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class CoordinatesController : ControllerBase
    {
        [HttpPost("log")]
        public ActionResult Log(float lon, float lat, [FromBody] string token)
        {
            return ActionService.Execute(this, "logCoordinates", () =>
            {
                return Ok(CoordinateService.LogCoordinates(JwtHandler.GetClaims(this), lon, lat, token));
            });
        }
    }
}
