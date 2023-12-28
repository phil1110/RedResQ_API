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
        }

        [HttpPost("add")]
        public ActionResult<long> Add(string city, string postalCode, long countryId)
        {
            try
            {
                JwtClaims claims = JwtHandler.GetClaims(this);

                long id = LocationService.Search(claims, city, postalCode, countryId);

                if(id > 0)
                {
                    return Ok(id);
                }
                else
                {
                    LocationService.Add(claims, city, postalCode, countryId);

                    id = LocationService.Search(claims, city, postalCode, countryId);

                    if( id > 0)
                    {
                        return Ok(id);
                    }
                    else
                    {
                        throw new Exception("Location was not added");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
        }
    }
}
