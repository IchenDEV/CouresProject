using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsMS.Models
{
    public class JsonResponse
    {

        public ResponseCode code { get; set; }
        public string msg { get; set; }
        public object data { get; set; }
        public int count { get; set; }
    }
    public enum ResponseCode
    {
        OK = 200,
        SQLError = 2,
        ArgError = 3,
        PasswordError=4
    }
    public class FailJsonResponse : JsonResponse
    {
        public FailJsonResponse(ResponseCode code, string msg = null, object data = null)
        {
            base.code = code;
            base.data = data;
            if (msg == null)
                base.msg = code.ToString();
            else
                base.msg = msg;
        }
    }
    public class SuccessJsonResponse : JsonResponse
    {
        public SuccessJsonResponse(object data = null)
        {
            base.data = data;
            code = ResponseCode.OK;
            msg = "OK";
        }
        public SuccessJsonResponse(int cx, object data = null)
        {
            base.count = cx;
            base.data = data;
            code = ResponseCode.OK;
            msg = "OK";
        }
    }
}
