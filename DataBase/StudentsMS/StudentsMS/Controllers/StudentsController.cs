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
    public class StudentsController : ControllerBase
    {

        // GET: api/<StudentsController>
        [HttpGet]
        public JsonResponse Get(int pageIndex, int pageSize)
        {
            if (pageSize == 0 || pageIndex == 0)
                return new SuccessJsonResponse(Student.Count(), Student.Gets());
            else
                return new SuccessJsonResponse(Student.Count(),Student.Gets(pageIndex, pageSize));
        }

        // GET api/<StudentsController>/5
        [HttpGet("{Sno}")]
        public JsonResponse Get(string Sno)
        {
            if (Sno == "" || Sno == null)
                return new FailJsonResponse(ResponseCode.ArgError);
            return new SuccessJsonResponse(Student.Get(Sno));
        }
        [HttpGet("{Sno}/score")]
        public JsonResponse GetScore(string Sno)
        {
            if (Sno == "" || Sno == null)
                return new FailJsonResponse(ResponseCode.ArgError);
            return new SuccessJsonResponse(Grade.GetGradesBySno(Sno));
        }
        [HttpGet("from")]
        public JsonResponse GetFrom()
        {
            return new SuccessJsonResponse(Student.GetGroupByFrom());
        }


        [HttpGet("{Sno}/gpa")]
        public JsonResponse GetGPA(string Sno)
        {
            if (Sno == "" || Sno == null)
                return new FailJsonResponse(ResponseCode.ArgError);
            return new SuccessJsonResponse(Gpa.GetGPA(Sno));
        }


        [HttpGet("{Sno}/year")]
        public JsonResponse GetYearScore(string Sno)
        {
            if (Sno == "" || Sno == null)
                return new FailJsonResponse(ResponseCode.ArgError);
            return new SuccessJsonResponse(GroupGrade.GetGroupByYear(Sno));
        }

        // GET api/<StudentsController>/5
        [HttpGet("{Sno}/class")]
        public JsonResponse GetClass(string Sno)
        {
            if (Sno == "" || Sno == null)
                return new FailJsonResponse(ResponseCode.ArgError);
            return new SuccessJsonResponse(Student.Get(Sno).GetStudentClass());
        }

        // POST api/<StudentsController>/insert
        [HttpPost("insert")]
        public JsonResponse PostInsert([FromBody] Student s1)
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

        // POST api/<StudentsController>/upadte
        [HttpPost("update")]
        public JsonResponse PostUpdate([FromBody] Student s1)
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


        // DELETE api/<StudentsController>/delete/5
        [HttpGet("delete/{Sno}")]
        public JsonResponse Delete(string Sno)
        {
            if (Sno == "" || Sno == null)
                return new FailJsonResponse(ResponseCode.ArgError);
            try
            {
                if (Student.Delete(Sno))
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
