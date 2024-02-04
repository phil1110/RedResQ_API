using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class QuizTypeController : ControllerBase
    {
        [HttpGet("fetch")]
        public ActionResult<QuizType[]> Fetch(int? amount, long? id, string? name)
        {
            return ActionService.Execute(this, "getQuizType", () =>
            {
                return Ok(QuizTypeService.Fetch(amount, id, name));
            });
        }

        [HttpGet("get")]
        public ActionResult<QuizType> Get(long id)
        {
            return ActionService.Execute(this, "getQuizType", () =>
            {
                return Ok(QuizTypeService.Get(id));
            });
        }

        [HttpPost("add")]
        public ActionResult<bool> Add(string name)
        {
            return ActionService.Execute(this, "addQuizType", () =>
            {
                return Ok(QuizTypeService.Add(name));
            });
        }

        [HttpPut("edit")]
        public ActionResult<bool> Edit(QuizType quizType)
        {
            return ActionService.Execute(this, "editQuizType", () =>
            {
                return Ok(QuizTypeService.Edit(quizType));
            });
        }

        [HttpDelete("delete")]
        public ActionResult<bool> Delete(long id)
        {
            return ActionService.Execute(this, "deleteQuizType", () =>
            {
                return Ok(QuizTypeService.Delete(id));
            });
        }
    }
}
