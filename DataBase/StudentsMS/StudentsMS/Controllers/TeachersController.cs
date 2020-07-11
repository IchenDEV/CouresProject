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
    public class TeachersController : ControllerBase
    {

        [HttpGet]
        public JsonResponse Get(int pageIndex, int pageSize)
        {
            if (pageSize == 0 || pageIndex == 0)
                return new SuccessJsonResponse(Teacher.Count(), Teacher.Gets());
            else
                return new SuccessJsonResponse(Teacher.Count(), Teacher.Gets(pageIndex, pageSize));
        }

        [HttpGet("{Tno}")]
        public JsonResponse Get(string Tno)
        {
            if (Tno == "" || Tno == null)
                return new FailJsonResponse(ResponseCode.ArgError);
            return new SuccessJsonResponse(Teacher.Get(Tno));
        }

        [HttpGet("{Tno}/course")]
        public JsonResponse GetCourses(string Tno, int pageIndex, int pageSize)
        {
            if (pageSize == 0 || pageIndex == 0)
                return new SuccessJsonResponse(Teacher.CountCourse(Tno), Teacher.GetCourses(Tno));
            else
                return new SuccessJsonResponse(Teacher.CountCourse(Tno), Teacher.GetCourses(Tno, pageIndex, pageSize));
        }

        [HttpGet("{Tno}/score")]
        public JsonResponse GetScore(string Tno, int pageIndex, int pageSize)
        {
            return new SuccessJsonResponse(Grade.GetGradesByTno(Tno));
        }


        // POST api/<StudentsController>/insert
        [HttpPost("insert")]
        public JsonResponse PostInsert([FromBody] Teacher s1)
        {
            if (s1.No == null)
                return new FailJsonResponse(ResponseCode.ArgError);
            try
            {
                if (s1.Insert())
                    return new SuccessJsonResponse();
                else
                    return new FailJsonResponse(ResponseCode.SQLError);
            }
            catch (Exception e)
            {
                return new FailJsonResponse(ResponseCode.SQLError, e.Message);
            }


        }

        // POST api/<StudentsController>/upadte
        [HttpPost("update")]
        public JsonResponse PostUpdate([FromBody] Teacher s1)
        {
            if (s1.No == null)
                return new FailJsonResponse(ResponseCode.ArgError);
            try
            {
                if (s1.Update())
                    return new SuccessJsonResponse();
                else
                    return new FailJsonResponse(ResponseCode.SQLError);
            }
            catch (Exception e)
            {
                return new FailJsonResponse(ResponseCode.SQLError, e.Message);
            }


        }


        // DELETE api/<StudentsController>/delete/5
        [HttpGet("delete/{Tno}")]
        public JsonResponse Delete(string Tno)
        {
            if (Tno == "" || Tno == null)
                return new FailJsonResponse(ResponseCode.ArgError);
            try
            {
                if (Teacher.Delete(Tno))
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
