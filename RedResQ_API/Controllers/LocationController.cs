using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class LocationController : ControllerBase
    {
        [HttpGet("get")]
        public ActionResult<Location> Get(long id)
        {
            try
            {
                return Ok(LocationService.Get(JwtHandler.GetClaims(this), id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            throw new NotImplementedException();
        }

        [HttpGet("search")]
        public ActionResult<long> Search(string city, string postalCode, long countryId)
        {
            try
            {
                long result = LocationService.Search(JwtHandler.GetClaims(this), city, postalCode, countryId);

                if(result > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            throw new NotImplementedException();
        }

        [HttpGet("fetch")]
        public ActionResult<Location> Fetch(long? id)
        {
            try
            {
                return Ok(LocationService.Fetch(JwtHandler.GetClaims(this), id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            throw new NotImplementedException();
        }

        [HttpPost("add")]
        public ActionResult<bool> Add(string city, string postalCode, long countryId)
        {
            try
            {
                return Ok(LocationService.Add(JwtHandler.GetClaims(this), city, postalCode, countryId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            throw new NotImplementedException();
        }

        [HttpPut("update")]
        public ActionResult<bool> Edit(Location loc)
        {
            try
            {
                return Ok(LocationService.Edit(JwtHandler.GetClaims(this), loc));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            throw new NotImplementedException();
        }

        [HttpDelete("delete")]
        public ActionResult<bool> Delete(long id)
        {
            try
            {
                return Ok(LocationService.Delete(JwtHandler.GetClaims(this), id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            throw new NotImplementedException();
        }
    }
}
