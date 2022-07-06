
using CouponService.Models.Dto;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RoutesSecurity;
using System;
using System.Collections.Generic;

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
                var response = new Response
                {
                    Status = false,
                    Message = message,
                    Code = statusCode
                };
                return response;
            }
            public static dynamic ExceptionResponse(Exception ex)
            {
                var response = new Response
                {
                    Status = false,
                    Message = CommonMessage.ExceptionMessage + ex.Message + "******** Stack Trace ***********" + ex.StackTrace,
                    Code = StatusCodes.Status500InternalServerError
                };
                return response;
            }
            public static dynamic SuccessResponse(string message, bool isCreated, int InsertedId = 0)
            {
                var response = new Response
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
        public class ReportResponse<T> : Response
        {
            public List<T> Data { get; set; }
        }

        public class GetReportResponce
        {
            public PromotionReportResponse data { get; set; }
        }

        public class GetResponse<T> : Response where T : class
        {
            public Pagination Pagination { get; set; }
            public List<T> Data { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public JObject Included { get; set; }
        }
        public class GetResponseById<T> : Response where T : class
        {
            public T Data { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public JObject Included { get; set; }
        }
        public class GetResponseApi<T> : Response where T : class
        {
            public List<T> Data { get; set; }
        }
    }
}
