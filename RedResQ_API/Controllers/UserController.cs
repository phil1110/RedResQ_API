using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
	public class UserController : ControllerBase
    {
        [HttpGet("fetch")]
        public ActionResult<User[]> Fetch(long? id, int? amount)
        {
            return ActionService.Execute(this, "fetchUsers", () =>
            {
                return Ok(UserService.Fetch(id, amount));
            });
        }

        [HttpGet("search")]
        public ActionResult<User[]> Search(string query)
        {
            return ActionService.Execute(this, "searchForUser", () =>
            {
                if (query.Length < 2)
                {
                    throw new Exception("A minimum of three letters is required to complete the search!");
                }

                return Ok(UserService.Search(query));
            });
        }

        [HttpGet("get")]
        public ActionResult<User> Get(long? id)
        {
            if (id.HasValue)
            {
                return ActionService.Execute(this, "getSpecificUser", () =>
                {
                    return Ok(UserService.GetSpecific(id.Value));
                });
            }
            else
            {
                return ActionService.Execute(this, "getPersonalUser", () =>
                {
                    return Ok(UserService.GetPersonal(JwtHandler.GetClaims(this)));
                });
            }
            
        }

        [HttpGet("check/username")]
        public ActionResult<bool> CheckUsername(string username)
        {
            return ActionService.Execute(this, "checkExistenceOfUsername", () =>
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
            return ActionService.Execute(this, "checkExistenceOfEmail", () =>
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
            return ActionService.Execute(this, "editUser", () =>
            {
                bool result = UserService.Edit(user);

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
            Role role = RoleService.Get(roleId);
            string permName = "promoteTo" + role.Name.Replace(" ", "");

            return ActionService.Execute(this, permName, () =>
            {
                bool result = UserService.Promote(userId, role);

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
            return ActionService.Execute(this, "deleteUser", () =>
            {
                bool result = UserService.Delete(id);

                if (result)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            });
        }
    }
}
