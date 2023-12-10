using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedResQ_API.Lib;
using RedResQ_API.Lib.Models;
using RedResQ_API.Lib.Services;
using System.Diagnostics.Metrics;

namespace RedResQ_API.Controllers
{
    [ApiController, Authorize, Route("[controller]")]
    public class CountryController : ControllerBase
    {
        [HttpGet("fetch")]
        public ActionResult<Country[]> GetAllCountries()
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
        public ActionResult<Country> GetCountry(long id)
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

        [HttpPost("add")]
        public ActionResult<bool> AddCountry(string countryName)
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
        public ActionResult<bool> AddCountryArray(string[] countryNames)
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
        public ActionResult<bool> EditCountry(Country country)
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
        public ActionResult<bool> DeleteCountry(long id)
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
