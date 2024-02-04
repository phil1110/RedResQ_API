using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Buffers.Text;

namespace RedResQ_API.Controllers
{
    [ApiController, Route("img"), Authorize]
    public class ImageController : ControllerBase
    {
        //[HttpGet]
        //public ActionResult<Image> Get(long id)
        //{
        //    return ActionService.Execute(this, () =>
        //    {
        //        return Ok(ImageService.Get(JwtHandler.GetClaims(this), id));
        //    });
        //}

        //[HttpPost("search")]
        //public ActionResult<long> Search([FromBody] string base64)
        //{
        //    return ActionService.Execute(this, () =>
        //    {
        //        return Ok(ImageService.Search(JwtHandler.GetClaims(this), base64));
        //    });
        //}

        //[HttpPost("add")]
        //public ActionResult<long> Add([FromBody] string base64)
        //{
        //    return ActionService.Execute(this, () =>
        //    {
        //        return Ok(ImageService.Add(JwtHandler.GetClaims(this), base64));
        //    });
        //}

        //[HttpDelete("delete")]
        //public ActionResult<bool> Delete(long id)
        //{
        //    return ActionService.Execute(this, () =>
        //    {
        //        return Ok(ImageService.Delete(JwtHandler.GetClaims(this), id));
        //    });
        //}
    }
}
