using RedResQ_API.Lib.Models;
using RedResQ_API.Lib.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Services
{
    public static class QuestionService
    {
        public static Question Get(JwtClaims claims, long quizId, long id)
        {
            if(PermissionService.IsPermitted("getQuestion", claims.Role))
            {

            }

            throw new NotFoundException();
        }
    }
}
