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
    public class ClassesController : ControllerBase
    {

        [HttpGet]
        public JsonResponse Get(int pageIndex, int pageSize)
        {
            if (pageSize == 0 || pageIndex == 0)
                return new SuccessJsonResponse(Class.Count(), Class.Gets());
            else
                return new SuccessJsonResponse(Class.Count(), Class.Gets(pageIndex, pageSize));
        }

        [HttpGet("{CLno}")]
        public JsonResponse Get(string CLno)
        {
            return new SuccessJsonResponse(Class.Get(CLno));
        }

        [HttpGet("{CLno}/students")]
        public JsonResponse GetStudents(string CLno, int pageIndex, int pageSize)
        {
            if (pageSize == 0 || pageIndex == 0)
                return new SuccessJsonResponse(Class.CountClassStudents(CLno), Class.GetsClassStudents(CLno));
            else
                return new SuccessJsonResponse(Class.CountClassStudents(CLno),Class.GetsClassStudents(CLno,pageIndex,pageSize));
        }

        [HttpGet("{CLno}/gpa")]
        public JsonResponse GetGpa(string CLno)
        {
           
                return new SuccessJsonResponse(Gpa.GetGPAByCLno(CLno));
        }

        [HttpGet("{CLno}/course")]
        public JsonResponse GetCourses(string CLno, int pageIndex, int pageSize)
        {
            if (pageSize == 0 || pageIndex == 0)
                return new SuccessJsonResponse(Class.CountClassCourses(CLno), Class.GetsClassCourses(CLno));
            else
                return new SuccessJsonResponse(Class.CountClassCourses(CLno), Class.GetsClassCourses(CLno, pageIndex, pageSize));
        }

        [HttpPost("insert")]
        public JsonResponse PostInsert([FromBody] Class data)
        {
            try
            {
                data.Insert();
                return new SuccessJsonResponse();
            }
            catch (Exception e)
            {
                return new FailJsonResponse(ResponseCode.SQLError, e.Message);
            }

        }

        [HttpPost("update")]
        public JsonResponse PostUpdate([FromBody] Class data)
        {
            try
            {
                data.Update();
                return new SuccessJsonResponse();
            }
            catch (Exception e)
            {
                return new FailJsonResponse(ResponseCode.SQLError, e.Message);
            }
           
        }


        // DELETE api/<StudentsController>/delete/5
        [HttpGet("delete/{CLno}")]
        public JsonResponse Delete(string CLno)
        {
            try
            {
                Class.Delete(CLno);
                return new SuccessJsonResponse();
            }
            catch (Exception e)
            {
                return new FailJsonResponse(ResponseCode.SQLError, e.Message);
            }

        }
    }
}
