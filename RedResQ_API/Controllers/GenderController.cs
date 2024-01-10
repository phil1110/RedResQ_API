using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedResQ_API.Lib.Exceptions;
using System.Xml.Linq;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class GenderController : ControllerBase
    {
        [HttpGet("fetch")]
        public ActionResult<Gender> GetAll()
        {
            return ActionService.Execute(this, () =>
            {
                return Ok(GenderService.GetAll(JwtHandler.GetClaims(this)));
            });
        }

        [HttpGet("get")]
        public ActionResult<Gender> Get(long id)
        {
            return ActionService.Execute(this, () =>
            {
                return Ok(GenderService.Get(JwtHandler.GetClaims(this), id));
            });
        }

        [HttpPost("add")]
        public ActionResult<bool> Add(string name)
        {
            return ActionService.Execute(this, () =>
            {
                return Ok(GenderService.Add(JwtHandler.GetClaims(this), name));
            });
        }

        [HttpPut("update")]
        public ActionResult<bool> Edit(Gender gender)
        {
            return ActionService.Execute(this, () =>
            {
                return Ok(GenderService.Edit(JwtHandler.GetClaims(this), gender));
            });
        }

        [HttpDelete("delete")]
        public ActionResult<bool> Delete(long id)
        {
            return ActionService.Execute(this, () =>
            {
                return Ok(GenderService.Delete(JwtHandler.GetClaims(this), id));
            });
        }
    }
}
