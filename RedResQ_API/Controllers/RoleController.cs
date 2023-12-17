using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class RoleController : ControllerBase
    {
        public ActionResult<Role> Get(long id)
        {
            try
            {
                return Ok(RoleService.Get(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public ActionResult<Role[]> Fetch()
        {
            try
            {
                return Ok(RoleService.Fetch());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
