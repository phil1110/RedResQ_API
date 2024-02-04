using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedResQ_API.Lib.Models;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class QuizController : ControllerBase
    {
        [HttpGet("fetch")]
        public ActionResult<Quiz[]> Fetch(long? id, int? amount, string? query, long? quizTypeId)
        {
            return ActionService.Execute(this, "getQuiz", () =>
            {
                return Ok(QuizService.Fetch(id, amount, query, quizTypeId));
            });
        }

        [HttpGet("get")]
        public ActionResult<Quiz> Get(long id)
        {
            return ActionService.Execute(this, "getQuiz", () =>
            {
                return Ok(QuizService.Get(id));
            });
        }

        [HttpPost("add")]
        public ActionResult<bool> Add(Quiz quiz)
        {
            return ActionService.Execute(this, "addQuiz", () =>
            {
                return Ok(QuizService.Add(quiz));
            });
        }

        [HttpPut("edit")]
        public ActionResult<bool> Edit(long id, string? name, int? maxScore, long? typeId)
        {
            return ActionService.Execute(this, "editQuiz", () =>
            {
                return Ok(QuizService.Edit(id, name, maxScore, typeId));
            });
        }

        [HttpDelete("delete")]
        public ActionResult<bool> Delete(long id)
        {
            return ActionService.Execute(this, "deleteQuiz", () =>
            {
                return Ok(QuizService.Delete(id));
            });
        }
    }
}
