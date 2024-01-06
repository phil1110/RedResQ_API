using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class PermissionController : ControllerBase
    {
        [HttpGet("get")]
        public ActionResult<Permission> GetPermission(string name)
        {
            try
            {
                Permission permission = PermissionService.GetPermission(JwtHandler.GetClaims(this), name);

                if (permission != null)
                {
                    return Ok(permission);
                }
                else
                {
                    return BadRequest("Permission was null!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("fetch")]
        public ActionResult<Permission[]> GetAllPermissions()
        {
            try
            {
                Permission[] permissions = PermissionService.GetAllPermissions(JwtHandler.GetClaims(this));

                if(permissions != null)
                {
                    return Ok(permissions);
                }
                else
                {
                    return BadRequest("No exisiting permissions!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("fetchForRole")]
        public ActionResult<Permission[]> GetAllPermissions(long roleId)
        {
            try
            {
                Permission[] permissions = PermissionService.GetAllPermissionsForRole(JwtHandler.GetClaims(this), roleId);

                if (permissions != null)
                {
                    return Ok(permissions);
                }
                else
                {
                    return BadRequest("No permissions for role with id: " + roleId);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public ActionResult<bool> UpdatePermission(string name, long role)
        {
            try
            {
                int rowsAffected = PermissionService.UpdatePermission(JwtHandler.GetClaims(this), name, role);

                if (rowsAffected == 1)
                {
                    return Ok(true);
                }

                return BadRequest(false + "" + rowsAffected);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
