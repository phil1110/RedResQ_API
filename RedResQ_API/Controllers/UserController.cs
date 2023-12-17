using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedResQ_API.Lib.Models;

namespace RedResQ_API.Controllers
{
	[ApiController, Route("[controller]"), Authorize]
	public class UserController : ControllerBase
    {
        [HttpGet("fetch")]
        public ActionResult<User[]> Fetch(long? id, int? amount)
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

        [HttpGet("search")]
        public ActionResult<User[]> Search(string query)
        {
            try
            {
                if(query.Length < 2)
                {
                    throw new Exception("A minimum of three letters is required to complete the search!");
                }

                return Ok(UserService.Search(JwtHandler.GetClaims(this), query));
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

        [HttpGet("check/username")]
        public ActionResult<bool> CheckUsername(string username)
        {
            try
            {
                bool result = UserService.CheckUsername(JwtHandler.GetClaims(this), username);

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

        [HttpGet("check/email")]
        public ActionResult<bool> CheckEmail(string email)
        {
            try
            {
                bool result = UserService.CheckEmail(JwtHandler.GetClaims(this), email);

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

        [HttpPut("promote")]
        public ActionResult<bool> Promote(long userId, long roleId)
        {
            try
            {
                bool result = UserService.Promote(JwtHandler.GetClaims(this), userId, roleId);

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
