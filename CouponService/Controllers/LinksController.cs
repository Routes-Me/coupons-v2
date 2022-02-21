using CouponService.Abstraction;
using CouponService.Models;
using CouponService.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using RoutesSecurity;
using System;
using static CouponService.Models.ResponseModel.Response;

namespace CouponService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/")]
    public class LinksController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public LinksController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("links")]
        public IActionResult Post(Link link)
        {
            try
            {
                if ((!string.IsNullOrEmpty(link.PromotionId.ToString())) && (!string.IsNullOrEmpty(link.Web) || !string.IsNullOrEmpty(link.Android) || !string.IsNullOrEmpty(link.Ios)))
                {
                    link.PromotionId = Obfuscation.Decode(link.PromotionId.ToString());
                    _unitOfWork.LinkRepository.Post(link);
                    _unitOfWork.Save();
                }
                return ReturnResponse.SuccessResponse(CommonMessage.LinksInsert,true);
            }
            catch (Exception ex)
            {
                return ReturnResponse.ExceptionResponse(ex);
            }
        }
    }
}
