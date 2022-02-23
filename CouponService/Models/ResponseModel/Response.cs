
using Microsoft.AspNetCore.Http;
using RoutesSecurity;
using System;

namespace CouponService.Models.ResponseModel
{
    public class Response
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
        public string Id { get; set; }
        public static class ReturnResponse
        {
            public static dynamic ErrorResponse(string message, int statusCode)
            {
                Response response = new Response
                {
                    Status = false,
                    Message = message,
                    Code = statusCode
                };
                return response;
            }
            public static dynamic ExceptionResponse(Exception ex)
            {
                Response response = new Response
                {
                    Status = false,
                    Message = CommonMessage.ExceptionMessage + ex.Message + "******** Stack Trace ***********" + ex.StackTrace,
                    Code = StatusCodes.Status500InternalServerError
                };
                return response;
            }
            public static dynamic SuccessResponse(string message, bool isCreated, int InsertedId = 0)
            {
                Response response = new Response
                {
                    Status = true,
                    Message = message,
                    Id = Obfuscation.Encode(InsertedId)
                };
                if (isCreated)
                    response.Code = StatusCodes.Status201Created;
                else
                    response.Code = StatusCodes.Status200OK;

                return response;
            }
        }
    }
}
