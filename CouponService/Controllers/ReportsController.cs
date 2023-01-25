using CouponService.Abstraction;
using CouponService.Models.Dto;
using CouponService.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoutesSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
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
                        var promotions = _unitOfWork.PromotionRepository.Get(null, x => x.Advertisement_Id == Convert.ToInt32(id), null, x => x.Coupon, x => x.Link).ToList();
                        if (promotions.Count == 0)
                            continue;

                        foreach (var promotion in promotions)
                        {
                            if (promotion.Type == PromotionType.Coupons)
                            {
                                var promotionCouponDto = new PromotionCouponDto
                                {
                                    Title = promotion.Title,
                                    Subtitle = promotion.Subtitle,
                                    Code = promotion.Code,
                                    CreatedAt = promotion.CreatedAt.ToString(),

                                    UsageLimit = promotion.UsageLimit,
                                    IsSharable = promotion.IsSharable,
                                    AdvertisementId = Obfuscation.Encode(Convert.ToInt32(promotion.Advertisement_Id)),
                                    InstitutionId = Obfuscation.Encode(Convert.ToInt32(promotion.Institution_Id)),
                                    PromotionId = Obfuscation.Encode(Convert.ToInt32(promotion.PromotionId)),
                                    Type = promotion.Type.ToString()
                                };
                                list.Add(promotionCouponDto);
                            }

                            if (promotion.Type == PromotionType.Links)
                            {
                                var promotionLinkDto = new PromotionLinkDto
                                {
                                    Title = promotion.Title,
                                    Subtitle = promotion.Subtitle,
                                    Code = promotion.Code,
                                    Type = promotion.Type.ToString(),
                                    Links = new LinkReadDto()
                                    {
                                        Web = promotion.Link.Web,
                                        Ios = promotion.Link.Ios,
                                        Android = promotion.Link.Android,
                                    },
                                    AdvertisementId = Obfuscation.Encode(Convert.ToInt32(promotion.Advertisement_Id)),
                                    InstitutionId = Obfuscation.Encode(Convert.ToInt32(promotion.Institution_Id)),
                                    PromotionId = Obfuscation.Encode(Convert.ToInt32(promotion.PromotionId))
                                };

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
