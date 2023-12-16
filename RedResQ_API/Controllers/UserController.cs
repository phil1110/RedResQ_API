using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedResQ_API.Lib.Models;

namespace RedResQ_API.Controllers
{
	[ApiController, Route("[controller]"), Authorize]
	public class UserController : ControllerBase
    {
        [HttpGet("fetch")]
        public ActionResult<User> Fetch(long? id, int? amount)
        {
            try
            {
                return Ok(UserService.Fetch(JwtHandler.GetClaims(this), id, amount));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get")]
        public ActionResult<User> Get(long? id)
        {
            try
            {
                if(id.HasValue)
                {
                    return Ok(UserService.GetAny(JwtHandler.GetClaims(this), id.Value));
                }
                else
                {
                    return Ok(UserService.GetPersonal(JwtHandler.GetClaims(this)));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public ActionResult<bool> Edit(User user)
        {
            try
            {
                bool result = UserService.Edit(JwtHandler.GetClaims(this), user);

                if (result)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        public ActionResult<bool> Delete(long id)
        {
            try
            {
                bool result = UserService.Delete(JwtHandler.GetClaims(this), id);

                if (result)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
