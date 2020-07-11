using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentsMS.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentsMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CCController : ControllerBase
    {
     
        // GET: api/<StudentsController>
        [HttpGet]
        public JsonResponse Get(int pageIndex, int pageSize)
        {
            if (pageSize == 0 || pageIndex == 0)
                return new SuccessJsonResponse(ClassCourse.Count(), ClassCourse.Gets());
            else
                return new SuccessJsonResponse(ClassCourse.Count(), ClassCourse.Gets(pageIndex, pageSize));
        }


        [HttpGet("{cc}")]
        public JsonResponse Get(string SCno)
        {
            if (SCno == "" || SCno == null)
                return new FailJsonResponse(ResponseCode.ArgError);
            return new SuccessJsonResponse(ClassCourse.Get(SCno));
        }

        [HttpPost("insert")]
        public JsonResponse PostInsert([FromBody] ClassCourse s1)
        {
         
            try
            {
                s1.Insert();
                return new SuccessJsonResponse();
            }
            catch (Exception e)
            {
                return new FailJsonResponse(ResponseCode.SQLError, e.Message);
            }
        }

        [HttpPost("update")]
        public JsonResponse PostUpdate([FromBody] ClassCourse s1)
        {
            if (s1.CC == null)
                return new FailJsonResponse(ResponseCode.ArgError);
            try
            {
                s1.Update();
                return new SuccessJsonResponse();
            }
            catch (Exception e)
            {
                return new FailJsonResponse(ResponseCode.SQLError, e.Message);
            }
        }

        [HttpGet("delete/{cc}")]
        public JsonResponse   Delete(string cc)
        {
            if (cc == "" || cc == null)
                return new FailJsonResponse(ResponseCode.ArgError);
            try
            {
                if (ClassCourse.Delete(cc))
                    return new SuccessJsonResponse();
                else
                    return new FailJsonResponse(ResponseCode.SQLError);
            }
            catch (Exception e)
            {
                return new FailJsonResponse(ResponseCode.SQLError, e.Message);
            }
        }
    }
}
