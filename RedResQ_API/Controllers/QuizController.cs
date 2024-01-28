using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class QuizController : ControllerBase
    {
        [HttpGet("get")]
        public ActionResult<Quiz> Get(long id)
        {
            return ActionService.Execute(this, () =>
            {
                return Ok(QuizService.Get(JwtHandler.GetClaims(this), id));
            });
        }
    }
}
