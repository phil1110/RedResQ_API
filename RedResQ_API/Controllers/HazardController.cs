using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class HazardController :ControllerBase
    {
        [HttpGet("get")]
        public ActionResult<Hazard> Get(long id)
        {
            return ActionService.Execute(this, "getHazard", () =>
            {
                return Ok(HazardService.Get(id));
            });
        }

        [HttpGet("fetch")]
        public ActionResult<Hazard[]> Fetch(long? id, int? amount)
        {
            return ActionService.Execute(this, "getHazard", () =>
            {
                return Ok(HazardService.Fetch(id, amount));
            });
        }

        [HttpPost("add")]
        public ActionResult<bool> Add(string title, double lat, double lon, int radius, int typeId)
        {
            return ActionService.Execute(this, "addHazard", () =>
            {
                return Ok(HazardService.Add(title, lat, lon, radius, typeId).Result);
            });
        }

        [HttpPut("edit")]
        public ActionResult<bool> Edit(long id, [FromBody] string? title, double? lat, double? lon, int? radius, int? typeId)
        {
            return ActionService.Execute(this, "editHazard", () =>
            {
                return Ok(HazardService.Edit(id, title, lat, lon, radius, typeId));
            });
        }

        [HttpDelete("delete")]
        public ActionResult<bool> Delete(long id)
        {
            return ActionService.Execute(this, "deleteHazard", () =>
            {
                return Ok(HazardService.Delete(id));
            });
        }
    }
}
