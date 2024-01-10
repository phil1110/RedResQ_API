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
            return ActionService.Execute(this, () =>
            {
                return Ok(RoleService.Get(JwtHandler.GetClaims(this), id));
            });
        }

        [HttpGet("fetch")]
        public ActionResult<Role[]> Fetch()
        {
            return ActionService.Execute(this, () =>
            {
                return Ok(RoleService.Fetch(JwtHandler.GetClaims(this)));
            });
        }
    }
}
