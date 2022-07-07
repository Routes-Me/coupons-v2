using CouponService.Abstraction;
using CouponService.Models;
using CouponService.Models.Dto;
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

        [HttpPost]
        [Route("advertisements/promotions/reports")]
        public ActionResult Reports(List<string> advertisementId)
        {
            var promotionReportResponse = new PromotionReportResponse();
            var promotionLinkDtoList = new List<PromotionLinkDto>();
            var promotionCouponDtoList = new List<PromotionCouponDto>();
            var list = new List<dynamic>();
            try
            {
                if (advertisementId.Count <= 0)
                    return StatusCode(StatusCodes.Status404NotFound);
                else
                {
                    foreach (var id in advertisementId)
                    {
                        var promotions = _unitOfWork.PromotionRepository.Get(null, x => x.Advertisement_Id == Convert.ToInt32(id), null, x => x.Coupon, x => x.Link);
                        if (promotions.Count == 0)
                            continue;

                        foreach (var promotion in promotions)
                        {
                            if (promotion.Type == PromotionType.Coupons)
                            {
                                var promotionCouponDto = new PromotionCouponDto();
                                promotionCouponDto.Title = promotion.Title;
                                promotionCouponDto.Subtitle = promotion.Subtitle;
                                promotionCouponDto.Code = promotion.Code;
                                promotionCouponDto.CreatedAt = promotion.CreatedAt.ToString();

                                promotionCouponDto.UsageLimit = promotion.UsageLimit;
                                promotionCouponDto.IsSharable = promotion.IsSharable;
                                promotionCouponDto.AdvertisementId = Obfuscation.Encode(Convert.ToInt32(promotion.Advertisement_Id));
                                promotionCouponDto.InstitutionId = Obfuscation.Encode(Convert.ToInt32(promotion.Institution_Id));
                                promotionCouponDto.PromotionId = Obfuscation.Encode(Convert.ToInt32(promotion.PromotionId));
                                promotionCouponDto.Type = promotion.Type.ToString();
                                list.Add(promotionCouponDto);
                            }

                            if (promotion.Type == PromotionType.Links)
                            {
                                var promotionLinkDto = new PromotionLinkDto();
                                promotionLinkDto.Title = promotion.Title;
                                promotionLinkDto.Subtitle = promotion.Subtitle;
                                promotionLinkDto.Code = promotion.Code;
                                promotionLinkDto.Link.Web = promotion.Link.Web;
                                promotionLinkDto.Link.Ios = promotion.Link.Ios;
                                promotionLinkDto.Link.Android =  promotion.Link.Android;
                                promotionLinkDto.Type = promotion.Type.ToString();
                                promotionLinkDto.AdvertisementId = Obfuscation.Encode(Convert.ToInt32(promotion.Advertisement_Id));
                                promotionLinkDto.InstitutionId = Obfuscation.Encode(Convert.ToInt32(promotion.Institution_Id));
                                promotionLinkDto.PromotionId = Obfuscation.Encode(Convert.ToInt32(promotion.PromotionId));

                                list.Add(promotionLinkDto);
                            }
                        }
                    }
                }
                promotionReportResponse.Data = list;
                return StatusCode(StatusCodes.Status200OK, promotionReportResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ReturnResponse.ExceptionResponse(ex));
            }
        }
    }
}
