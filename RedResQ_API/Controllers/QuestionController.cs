using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RedResQ_API.Lib.Models;
using static System.Net.Mime.MediaTypeNames;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class QuestionController : ControllerBase
    {
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
