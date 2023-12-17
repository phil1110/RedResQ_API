using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class RoleController : ControllerBase
    {
        [HttpGet("get")]
        public ActionResult<Role> Get(long id)
        {
            try
            {
                return Ok(RoleService.Get(JwtHandler.GetClaims(this), id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("fetch")]
        public ActionResult<Role[]> Fetch()
        {
            try
            {
                return Ok(RoleService.Fetch(JwtHandler.GetClaims(this)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
