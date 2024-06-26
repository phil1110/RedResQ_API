﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class AttemptController : ControllerBase
    {
        [HttpGet("result")]
        public ActionResult<int> GetResult(long quizId, long attemptId)
        {
            return ActionService.Execute(this, "getResult", () =>
            {
                var claims = JwtHandler.GetClaims(this);

                return Ok(AttemptService.GetResult(attemptId, quizId, claims.Id));
            });
        }

        [HttpPost("submit")]
        public ActionResult<long> Add(GivenAnswer[] givenAnswers, long quizId)
        {
            return ActionService.Execute(this, "addAttempt", () =>
            {
                var claims = JwtHandler.GetClaims(this);

                return Ok(AttemptService.Add(claims, new Attempt(quizId, claims.Id, givenAnswers)));
            });
        }
    }
}
