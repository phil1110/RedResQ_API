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
            return ActionService.Execute(this, () =>
            {
                return Ok(UserService.Fetch(JwtHandler.GetClaims(this), id, amount));
            });
        }

        [HttpGet("search")]
        public ActionResult<User[]> Search(string query)
        {
            return ActionService.Execute(this, () =>
            {
                if (query.Length < 2)
                {
                    throw new Exception("A minimum of three letters is required to complete the search!");
                }

                return Ok(UserService.Search(JwtHandler.GetClaims(this), query));
            });
        }

        [HttpGet("get")]
        public ActionResult<User> Get(long? id)
        {
            return ActionService.Execute(this, () =>
            {
                if (id.HasValue)
                {
                    return Ok(UserService.GetAny(JwtHandler.GetClaims(this), id.Value));
                }
                else
                {
                    return Ok(UserService.GetPersonal(JwtHandler.GetClaims(this)));
                }
            });
        }

        [HttpGet("check/username")]
        public ActionResult<bool> CheckUsername(string username)
        {
            return ActionService.Execute(this, () =>
            {
                bool result = UserService.CheckUsername(JwtHandler.GetClaims(this), username);

                if (result)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            });
        }

        [HttpGet("check/email")]
        public ActionResult<bool> CheckEmail(string email)
        {
            return ActionService.Execute(this, () =>
            {
                bool result = UserService.CheckEmail(JwtHandler.GetClaims(this), email);

                if (result)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            });
        }

        [HttpPut("update")]
        public ActionResult<bool> Edit(User user)
        {
            return ActionService.Execute(this, () =>
            {
                bool result = UserService.Edit(JwtHandler.GetClaims(this), user);

                if (result)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            });
        }

        [HttpPut("promote")]
        public ActionResult<bool> Promote(long userId, long roleId)
        {
            return ActionService.Execute(this, () =>
            {
                bool result = UserService.Promote(JwtHandler.GetClaims(this), userId, roleId);

                if (result)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            });
        }

        [HttpDelete("delete")]
        public ActionResult<bool> Delete(long id)
        {
            return ActionService.Execute(this, () =>
            {
                bool result = UserService.Delete(JwtHandler.GetClaims(this), id);

                if (result)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            });
        }
    }
}
