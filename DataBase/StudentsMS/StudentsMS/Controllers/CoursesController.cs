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
    public class CoursesController : ControllerBase
    {
        [HttpGet]
        public JsonResponse Get(int pageIndex, int pageSize)
        {
            if (pageSize == 0 || pageIndex == 0)
                return new SuccessJsonResponse(Course.Count(), Course.Gets());
            else
                return new SuccessJsonResponse(Course.Count(), Course.Gets(pageIndex, pageSize));
        }

        [HttpGet("{Cno}")]
        public JsonResponse Get(string Cno)
        {
            if (Cno == "" || Cno == null)
                return new FailJsonResponse(ResponseCode.ArgError);
            return new SuccessJsonResponse(Course.Get(Cno));
        }
        [HttpGet("{Cno}/score")]
        public JsonResponse GetScore(string Cno)
        {
            if (Cno == "" || Cno == null)
                return new FailJsonResponse(ResponseCode.ArgError);
            return new SuccessJsonResponse(Grade.GetGradesByCno(Cno));
        }
        [HttpGet("{Cno}/averageScore")]
        public JsonResponse GetAverageScore(string Cno)
        {
            if (Cno == "" || Cno == null)
                return new FailJsonResponse(ResponseCode.ArgError);
            return new SuccessJsonResponse(Grade.GetGradesAvgByCno(Cno));
        }

        [HttpPost("insert")]
        public JsonResponse PostInsert([FromBody] Course s1)
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
        public JsonResponse PostUpdate([FromBody] Course s1)
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

        [HttpGet("delete/{Cno}")]
        public JsonResponse Delete(string Cno)
        {

            if (Cno == "" || Cno == null)
                return new FailJsonResponse(ResponseCode.ArgError);
            try
            {
                if (Course.Delete(Cno))
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
