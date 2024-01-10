using Microsoft.AspNetCore.Mvc;
using RedResQ_API.Lib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Services
{
    public static class ActionService
    {
        public static ActionResult Execute(ControllerBase controller, Func<ActionResult> func)
        {
            try
            {
                return func();
            }
            catch (AuthException ex)
            {
                return controller.Unauthorized(ex.Message);
            }
            catch (ForbidException ex)
            {
                return controller.Forbid(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return controller.NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return controller.UnprocessableEntity(ex.Message);
            }
            catch (Exception ex)
            {
                return controller.BadRequest(ex.Message);
            }
        }
    }
}
