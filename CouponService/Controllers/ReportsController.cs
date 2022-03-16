using CouponService.Abstraction;
using CouponService.Models;
using CouponService.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoutesSecurity;
using System;
using System.Collections.Generic;
using static CouponService.Models.ResponseModel.Response;

namespace CouponService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/")]
    public class ReportsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReportsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        [Route("advertisements/promotions/reports")]
        public ActionResult Reports(List<string> advertisementId)
        {
            try
            {
                if (advertisementId.Count <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, ReturnResponse.ErrorResponse(CommonMessage.InvalidData, 400));
                else
                {
                    var reports = new List<Report>();
                    foreach (var id in advertisementId)
                    {
                        
                        reports = (List<Report>)_unitOfWork.ReportRepository.GetReports(x => x.AdvertisementId == Obfuscation.Decode(id), x => x.Coupon, x => x.Link);
                    }
                   
                    return StatusCode(StatusCodes.Status200OK, reports);
                    //return StatusCode(StatusCodes.Status200OK, ReturnResponse.SuccessResponse(CommonMessage.PromotionsRetrived, false));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ReturnResponse.ExceptionResponse(ex));
                throw;
            }
        }
    }
}
