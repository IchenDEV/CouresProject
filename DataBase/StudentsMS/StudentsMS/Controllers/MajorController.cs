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
    public class MajorsController : ControllerBase
    {
     
        // GET: api/<StudentsController>
        [HttpGet]
        public JsonResponse Get(int pageIndex, int pageSize)
        {
            if (pageSize == 0 || pageIndex == 0)
                return new SuccessJsonResponse(Major.Count(), Major.Gets());
            else
                return new SuccessJsonResponse(Major.Count(), Major.Gets(pageIndex, pageSize));
        }


        [HttpGet("{Mno}")]
        public JsonResponse Get(string Mno)
        {
            if (Mno == "" || Mno == null)
                return new FailJsonResponse(ResponseCode.ArgError);
            return new SuccessJsonResponse(Major.Get(Mno));
        }

        [HttpPost("insert")]
        public JsonResponse PostInsert([FromBody] Major s1)
        {
            if (s1.No == null)
                return new FailJsonResponse(ResponseCode.ArgError);
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
        public JsonResponse PostUpdate([FromBody] Major s1)
        {
            if (s1.No == null)
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

        [HttpGet("delete/{Mno}")]
        public JsonResponse   Delete(string Mno)
        {
            if (Mno == "" || Mno == null)
                return new FailJsonResponse(ResponseCode.ArgError);
            try
            {
                if (Major.Delete(Mno))
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
