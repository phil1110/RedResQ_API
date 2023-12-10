﻿using Microsoft.AspNetCore.Mvc;

namespace RedResQ_API.Controllers
{
	public class UserController : ControllerBase
	{
		[HttpGet("reset/request")]
		public ActionResult RequestPasswordReset(string email)
		{
			try
			{
				bool wasSuccessful = UserService.RequestReset(email);

				if (wasSuccessful)
				{
					return Ok("Successfully requested password reset!");
				}
				else
				{
					return BadRequest("This point should be unreachable.");
				}
			}
			catch (Exception ex)
			{
				return BadRequest("Error! Message: " + ex.Message);
			}
		}

		[HttpGet("reset/confirm")]
		public ActionResult ConfirmPasswordReset(int confirmationCode, string email,  string password)
		{
            try
            {
                bool wasSuccessful = UserService.ConfirmReset(confirmationCode, email, password);

                if (wasSuccessful)
                {
                    return Ok("Password Request was successful!");
                }
                else
                {
                    return BadRequest("Password Request was not successful.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error! Message: " + ex.Message);
            }
        }

		[HttpGet("reset/verify")]
		public ActionResult<bool> CheckValidity(int code, string email)
		{
			try
			{
				bool isValid = UserService.CheckValidity(code, email);

				return Ok(isValid);
            }
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
