using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]")]
    public class GenderController : ControllerBase
    {
        [HttpGet("fetch")]
        public ActionResult<Gender> GetAll()
        {
            try
            {
                return Ok(GenderService.GetAll());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get")]
        public ActionResult<Gender> Get(long id)
        {
            try
            {
                return Ok(GenderService.Get(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add"), Authorize]
        public ActionResult<bool> Add(string name)
        {
            try
            {
                return Ok(GenderService.Add(JwtHandler.GetClaims(this), name));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update"), Authorize]
        public ActionResult<bool> Edit(Gender gender)
        {
            try
            {
                return Ok(GenderService.Edit(JwtHandler.GetClaims(this), gender));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete"), Authorize]
        public ActionResult<bool> Delete(long id)
        {
            try
            {
                return Ok(GenderService.Delete(JwtHandler.GetClaims(this), id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
