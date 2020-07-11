using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentsMS.Models;


namespace StudentsMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        [HttpPost("admin")]
        public JsonResponse PostLogin([FromBody] User u)
        {
            if (u.username == "Admin" && u.pwd == "123456")
            {
                return new SuccessJsonResponse("OK");
            }
            return new FailJsonResponse(ResponseCode.PasswordError);
        }

        [HttpPost("stu")]
        public JsonResponse PostLoginStu([FromBody] User u)
        {
            return new SuccessJsonResponse("OK");
        }

        [HttpPost("teacher")]
        public JsonResponse PostLoginTeacher([FromBody] User u)
        {

            return new SuccessJsonResponse("OK");

        }
    }
    public class User
    {
        public string username;
        public string pwd;
    }
}
