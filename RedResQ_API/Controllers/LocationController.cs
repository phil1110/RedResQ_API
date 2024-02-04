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
            return ActionService.Execute(this, "getLocation", () =>
            {
                return Ok(LocationService.Get(id));
            });
        }

        [HttpGet("search")]
        public ActionResult<long> Search(string city, string postalCode, long countryId)
        {
            return ActionService.Execute(this, "searchLocation", () =>
            {
                long result = LocationService.Search(city, postalCode, countryId);

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
            return ActionService.Execute(this, "getLocation", () =>
            {
                return Ok(LocationService.Fetch(JwtHandler.GetClaims(this), id));
            });
        }

        [HttpPost("add")]
        public ActionResult<long> Add(string city, string postalCode, long countryId)
        {
            return ActionService.Execute(this, "addLocation", () =>
            {
                JwtClaims claims = JwtHandler.GetClaims(this);

                long id = LocationService.Search(city, postalCode, countryId);

                if (id > 0)
                {
                    return Ok(id);
                }
                else
                {
                    LocationService.Add(city, postalCode, countryId);

                    id = LocationService.Search(city, postalCode, countryId);

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
            return ActionService.Execute(this, "editLocation", () =>
            {
                return Ok(LocationService.Edit(loc));
            });
        }

        [HttpDelete("delete")]
        public ActionResult<bool> Delete(long id)
        {
            return ActionService.Execute(this, "deleteLocation", () =>
            {
                return Ok(LocationService.Delete(id));
            });
        }
    }
}
