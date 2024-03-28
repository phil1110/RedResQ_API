using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class QuestionController : ControllerBase
    {
        [HttpGet("fetch")]
        public ActionResult<Question[]> Fetch(long quizId)
        {
            return ActionService.Execute(this, "getQuestion", () =>
            {
                return Ok(QuestionService.Fetch(quizId));
            });
        }

        [HttpGet("get")]
        public ActionResult<Question> Get(long quizId, long id)
        {
            return ActionService.Execute(this, "getQuestion", () =>
            {
                return Ok(QuestionService.Get(quizId, id));
            });
        }

        [HttpPost("add")]
        public ActionResult<bool> Add(Question question)
        {
            return ActionService.Execute(this, "addQuestion", () =>
            {
                return Ok(QuestionService.Add(question));
            });
        }

        [HttpPut("edit")]
        public ActionResult<bool> Edit(long quizId, long id, [FromBody] string text)
        {
            return ActionService.Execute(this, "editQuestion", () =>
            {
                return Ok(QuestionService.Edit(quizId, id, text));
            });
        }

        [HttpDelete("delete")]
        public ActionResult Delete(long quizId, long id)
        {
            return ActionService.Execute(this, "deleteQuestion", () =>
            {
                return Ok(QuestionService.Delete(quizId, id));
            });
        }
    }
}
