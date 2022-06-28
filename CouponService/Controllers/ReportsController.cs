using CouponService.Abstraction;
using CouponService.Models;
using CouponService.Models.Dto;
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
            var promotionReportResponse = new PromotionReportResponse();
            var promotionLinkDtoList = new List<PromotionLinkDto>();
            var promotionCouponDtoList = new List<PromotionCouponDto>();
            var list = new List<dynamic>();
            try
            {
                if (advertisementId.Count <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, ReturnResponse.ErrorResponse(CommonMessage.InvalidData, 400));
                else
                {
                    foreach (var id in advertisementId)
                    {
                        var promotion = _unitOfWork.PromotionRepository.GetById(x => x.Advertisement_Id == Convert.ToInt32(id), null, x => x.Coupon, x => x.Link);

                        if (promotion == null)
                            throw new Exception(CommonMessage.PromotionsNotFound);

                        if (promotion.Type.ToString() == "coupons")
                        {
                            var promotionCouponDto = new PromotionCouponDto();
                            promotionCouponDto.Title = promotion.Title;
                            promotionCouponDto.Subtitle = promotion.Subtitle;
                            promotionCouponDto.Code = promotion.Code;
                            promotionCouponDto.StartAt = promotion.StartAt;
                            promotionCouponDto.EndAt = promotion.EndAt;
                            promotionCouponDto.UsageLimit = promotion.UsageLimit;
                            promotionCouponDto.IsSharable = promotion.IsSharable;
                            promotionCouponDto.AdvertisementId = Obfuscation.Encode(Convert.ToInt32(promotion.AdvertisementId));
                            promotionCouponDto.InstitutionId = Obfuscation.Encode(Convert.ToInt32(promotion.InstitutionId));
                            promotionCouponDto.Type = promotion.Type.ToString();
                            //promotionCouponDtoList.Add(promotionCouponDto);
                            list.Add(promotionCouponDto);
                        }

                        if (promotion.Type.ToString() == "links")
                        {
                            var promotionLinkDto = new PromotionLinkDto();
                            promotionLinkDto.Title = promotion.Title;
                            promotionLinkDto.Subtitle = promotion.Subtitle;
                            promotionLinkDto.Code = promotion.Code;
                            promotionLinkDto.Link.Web = promotion.Link.Web == null ? "" : promotion.Link.Web;
                            promotionLinkDto.Link.Ios = promotion.Link.Ios == null ? "" : promotion.Link.Ios;
                            promotionLinkDto.Link.Android = promotion.Link.Android == null ? "" : promotion.Link.Android;
                            promotionLinkDto.Type = promotion.Type.ToString();
                            promotionLinkDto.AdvertisementId = Obfuscation.Encode(Convert.ToInt32(promotion.AdvertisementId));
                            promotionLinkDto.InstitutionId = Obfuscation.Encode(Convert.ToInt32(promotion.InstitutionId));
                            //promotionLinkDtoList.Add(promotionLinkDto);
                            list.Add(promotionLinkDto);
                        }

                    }
                }

                //promotionReportResponse.Links = promotionLinkDtoList;
                //promotionReportResponse.Coupons = promotionCouponDtoList;
                //responce.data = promotionReportResponse;
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
