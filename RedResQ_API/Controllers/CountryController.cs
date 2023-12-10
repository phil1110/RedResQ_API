using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]")]
    public class CountryController : ControllerBase
    {
        [HttpGet("fetch")]
        public ActionResult<Country[]> GetAll()
        {
            try
            {
                return Ok(CountryService.GetAllCountries());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get")]
        public ActionResult<Country> Get(long id)
        {
            try
            {
                return Ok(CountryService.GetCountry(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add"), Authorize]
        public ActionResult<bool> Add(string countryName)
        {
            try
            {
                return Ok(CountryService.AddCountry(JwtHandler.GetClaims(this), countryName));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add/list"), Authorize]
        public ActionResult<bool> AddArray(string[] countryNames)
        {
            try
            {
                return Ok(CountryService.AddCountryArray(JwtHandler.GetClaims(this), countryNames));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update"), Authorize]
        public ActionResult<bool> Edit(Country country)
        {
            try
            {
                return Ok(CountryService.EditCountry(JwtHandler.GetClaims(this), country));
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
                return Ok(CountryService.DeleteCountry(JwtHandler.GetClaims(this), id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
