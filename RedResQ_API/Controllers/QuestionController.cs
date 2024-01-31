using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class QuestionController
    {
        [HttpGet("get")]
        public ActionResult<Question> Get()
        {
            throw new NotImplementedException();
        }
    }
}
