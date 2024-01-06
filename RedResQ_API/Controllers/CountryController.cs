using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class CountryController : ControllerBase
    {
        [HttpGet("fetch")]
        public ActionResult<Country[]> GetAll()
        {
            try
            {
                return Ok(CountryService.GetAllCountries(JwtHandler.GetClaims(this)));
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
                return Ok(CountryService.GetCountry(JwtHandler.GetClaims(this), id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add")]
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

        [HttpPost("add/list")]
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

        [HttpPut("update")]
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

        [HttpDelete("delete")]
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
