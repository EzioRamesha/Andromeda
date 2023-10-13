using Shared.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Controllers
{
    public class EmailController : BaseController
    {
        public static Result Create(int receipentUserId, string subject, string content, bool send = true)
        {
            Result result = new Result();




            return result;
        }


    }
}