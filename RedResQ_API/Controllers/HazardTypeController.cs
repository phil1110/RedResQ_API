using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class HazardTypeController : ControllerBase
    {
        [HttpGet("fetch")]
        public ActionResult<HazardType[]> Fetch()
        {
            return ActionService.Execute(this, "getHazardType", () =>
            {
                return Ok(HazardTypeService.Fetch());
            });
        }

        [HttpPost("add")]
        public ActionResult<bool> Add([FromBody] string name)
        {
            return ActionService.Execute(this, "addHazardType", () =>
            {
                return Ok(HazardTypeService.Add(name));
            });
        }

        [HttpPut("edit")]
        public ActionResult<bool> Edit(HazardType hazardType)
        {
            return ActionService.Execute(this, "editHazardType", () =>
            {
                return Ok(HazardTypeService.Edit(hazardType));
            });
        }

        [HttpDelete("delete")]
        public ActionResult<bool> Delete(int id)
        {
            return ActionService.Execute(this, "deleteHazardType", () =>
            {
                return Ok(HazardTypeService.Delete(id));
            });
        }
    }
}
