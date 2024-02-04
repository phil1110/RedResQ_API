using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedResQ_API.Lib.Exceptions;
using System.Diagnostics.Metrics;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class CountryController : ControllerBase
    {
        [HttpGet("fetch")]
        public ActionResult<Country[]> GetAll()
        {
            return ActionService.Execute(this, "getCountry", () =>
            {
                return Ok(CountryService.GetAllCountries());
            });
        }

        [HttpGet("get")]
        public ActionResult<Country> Get(long id)
        {
            return ActionService.Execute(this, "getCountry", () =>
            {
                return Ok(CountryService.GetCountry(id));
            });
        }

        [HttpPost("add")]
        public ActionResult<bool> Add(string countryName)
        {
            return ActionService.Execute(this, "addCountry", () =>
            {
                return Ok(CountryService.AddCountry(JwtHandler.GetClaims(this), countryName));
            });
        }

        [HttpPost("add/list")]
        public ActionResult<bool> AddArray(string[] countryNames)
        {
            return ActionService.Execute(this, "addCountry", () =>
            {
                return Ok(CountryService.AddCountryArray(JwtHandler.GetClaims(this), countryNames));
            });
        }

        [HttpPut("update")]
        public ActionResult<bool> Edit(Country country)
        {
            return ActionService.Execute(this, "editCountry", () =>
            {
                return Ok(CountryService.EditCountry(country));
            });
        }

        [HttpDelete("delete")]
        public ActionResult<bool> Delete(long id)
        {
            return ActionService.Execute(this, "deleteCountry", () =>
            {
                return Ok(CountryService.DeleteCountry(id));
            });
        }
    }
}
