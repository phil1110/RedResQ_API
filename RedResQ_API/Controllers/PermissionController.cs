using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedResQ_API.Lib;
using RedResQ_API.Lib.Models;
using RedResQ_API.Lib.Services;

namespace RedResQ_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PermissionController : ControllerBase
    {
        [HttpGet("get")]
        public ActionResult<Permission> GetPermission(string name)
        {
            try
            {
                Permission permission = PermissionService.GetPermission(name);

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
                Permission[] permissions = PermissionService.GetAllPermissions();

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

        [HttpGet("fetchRole")]
        public ActionResult<Permission[]> GetAllPermissions(long roleId)
        {
            try
            {
                Permission[] permissions = PermissionService.GetAllPermissionsForRole(roleId);

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

        [HttpPut("set")]
        [Authorize]
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
