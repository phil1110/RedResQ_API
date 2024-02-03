using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class AnswerController : ControllerBase
    {
        [HttpGet("fetch")]
        public ActionResult<Answer[]> Fetch(long quizId, long questionId)
        {
            return ActionService.Execute(this, "getAnswer", () =>
            {
                return Ok(AnswerService.Fetch(quizId, questionId));
            });
        }

        [HttpGet("get")]
        public ActionResult<Answer[]> Get(long quizId, long questionId, long id)
        {
            return ActionService.Execute(this, "getAnswer", () =>
            {
                return Ok(AnswerService.Get(quizId, questionId, id));
            });
        }

        [HttpPost("add")]
        public ActionResult<Answer[]> Add(Answer answer)
        {
            return ActionService.Execute(this, "addAnswer", () =>
            {
                return Ok(AnswerService.Add(answer));
            });
        }

        [HttpPut("edit")]
        public ActionResult<Answer[]> Edit(Answer answer)
        {
            return ActionService.Execute(this, "editAnswer", () =>
            {
                return Ok(AnswerService.Edit(answer));
            });
        }

        [HttpDelete("delete")]
        public ActionResult<Answer[]> Delete(long quizId, long questionId, long id)
        {
            return ActionService.Execute(this, "deleteAnswer", () =>
            {
                return Ok(AnswerService.Delete(quizId, questionId, id));
            });
        }
    }
}
