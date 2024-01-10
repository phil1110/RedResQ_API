﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class ResetController : ControllerBase
    {
        [HttpGet("request")]
        public ActionResult RequestPasswordReset(string email)
        {
            return ActionService.Execute(this, () =>
            {
                bool wasSuccessful = ResetService.RequestReset(JwtHandler.GetClaims(this), email);

                if (wasSuccessful)
                {
                    return Ok("Successfully requested password reset!");
                }
                else
                {
                    return BadRequest("This point should be unreachable.");
                }
            });
        }

        [HttpGet("confirm")]
        public ActionResult ConfirmPasswordReset(int confirmationCode, string email, string password)
        {
            return ActionService.Execute(this, () =>
            {
                bool wasSuccessful = ResetService.ConfirmReset(JwtHandler.GetClaims(this), confirmationCode, email, password);

                if (wasSuccessful)
                {
                    return Ok("Password Request was successful!");
                }
                else
                {
                    return BadRequest("Password Request was not successful.");
                }
            });
        }

        [HttpGet("verify")]
        public ActionResult<bool> CheckValidity(int code, string email)
        {
            return ActionService.Execute(this, () =>
            {
                bool isValid = ResetService.CheckValidity(JwtHandler.GetClaims(this), code, email);

                return Ok(isValid);
            });
        }
    }
}
