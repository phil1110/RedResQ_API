using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedResQ_API.Lib.Exceptions;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class LocationController : ControllerBase
    {
        [HttpGet("get")]
        public ActionResult<Location> Get(long id)
        {
            return ActionService.Execute(this, () =>
            {
                return Ok(LocationService.Get(JwtHandler.GetClaims(this), id));
            });
        }

        [HttpGet("search")]
        public ActionResult<long> Search(string city, string postalCode, long countryId)
        {
            return ActionService.Execute(this, () =>
            {
                long result = LocationService.Search(JwtHandler.GetClaims(this), city, postalCode, countryId);

                if (result > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(result);
                }
            });
        }

        [HttpGet("fetch")]
        public ActionResult<Location> Fetch(long? id)
        {
            return ActionService.Execute(this, () =>
            {
                return Ok(LocationService.Fetch(JwtHandler.GetClaims(this), id));
            });
        }

        [HttpPost("add")]
        public ActionResult<long> Add(string city, string postalCode, long countryId)
        {
            return ActionService.Execute(this, () =>
            {
                JwtClaims claims = JwtHandler.GetClaims(this);

                long id = LocationService.Search(claims, city, postalCode, countryId);

                if (id > 0)
                {
                    return Ok(id);
                }
                else
                {
                    LocationService.Add(claims, city, postalCode, countryId);

                    id = LocationService.Search(claims, city, postalCode, countryId);

                    if (id > 0)
                    {
                        return Ok(id);
                    }
                    else
                    {
                        throw new Exception("Location was not added");
                    }
                }
            });
        }

        [HttpPut("update")]
        public ActionResult<bool> Edit(Location loc)
        {
            return ActionService.Execute(this, () =>
            {
                return Ok(LocationService.Edit(JwtHandler.GetClaims(this), loc));
            });
        }

        [HttpDelete("delete")]
        public ActionResult<bool> Delete(long id)
        {
            return ActionService.Execute(this, () =>
            {
                return Ok(LocationService.Delete(JwtHandler.GetClaims(this), id));
            });
        }
    }
}
