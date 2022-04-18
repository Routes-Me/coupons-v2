using CouponService.Abstraction;
using CouponService.Models;
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


        [HttpGet]
        [Route("advertisements/promotions/reports")]
        public ActionResult Reports(List<string> advertisementId)
        {
            GetReportResponce responce = new GetReportResponce();
            PromotionReportResponce promotionReportResponce = new PromotionReportResponce();
            List<PromotionLinkDto> promotionLinkDtoList = new List<PromotionLinkDto>();
            List<PromotionCouponDto> promotionCouponDtoList = new List<PromotionCouponDto>();
            List<dynamic> list = new List<dynamic>();
            try
            {
                if (advertisementId.Count <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, ReturnResponse.ErrorResponse(CommonMessage.InvalidData, 400));
                else
                {
                    foreach (var id in advertisementId)
                    {
                        Promotion promotion = _unitOfWork.PromotionRepository.GetById(x => x.Advertisement_Id == Convert.ToInt32(id), null, x => x.Coupons, x => x.Links);

                        if (promotion == null)
                            throw new Exception(CommonMessage.PromotionsNotFound);

                        if(promotion.Type.ToString() == "coupons")
                        {
                            PromotionCouponDto promotionCouponDto = new PromotionCouponDto();
                            promotionCouponDto.Title = promotion.Title;
                            promotionCouponDto.Subtitle = promotion.Subtitle;
                            promotionCouponDto.code = promotion.Code;
                            promotionCouponDto.StartAt = promotion.StartAt;
                            promotionCouponDto.EndAt = promotion.EndAt;
                            promotionCouponDto.UsageLimit = promotion.UsageLimit;
                            promotionCouponDto.isSharable = promotion.IsSharable;
                            promotionCouponDto.AdvertisementId = Obfuscation.Encode(Convert.ToInt32(promotion.AdvertisementId));
                            promotionCouponDto.InstitutionId = Obfuscation.Encode(Convert.ToInt32(promotion.InstitutionId));
                            promotionCouponDto.type = promotion.Type.ToString();
                            //promotionCouponDtoList.Add(promotionCouponDto);
                            list.Add(promotionCouponDto);
                        }

                        if(promotion.Type.ToString() == "links")
                        {
                            PromotionLinkDto promotionLinkDto = new PromotionLinkDto();
                            promotionLinkDto.Title = promotion.Title;
                            promotionLinkDto.Subtitle = promotion.Subtitle;
                            promotionLinkDto.code = promotion.Code;
                            promotionLinkDto.link.Web = promotion.Links.Web == null ? "" : promotion.Links.Web;
                            promotionLinkDto.link.Ios = promotion.Links.Ios == null ? "" : promotion.Links.Ios;
                            promotionLinkDto.link.Android = promotion.Links.Android == null ? "" : promotion.Links.Android;
                            promotionLinkDto.type = promotion.Type.ToString();
                            promotionLinkDto.AdvertisementId = Obfuscation.Encode(Convert.ToInt32(promotion.AdvertisementId));
                            promotionLinkDto.InstitutionId = Obfuscation.Encode(Convert.ToInt32(promotion.InstitutionId));
                            //promotionLinkDtoList.Add(promotionLinkDto);
                            list.Add(promotionLinkDto);
                        }

                    }
                }

                //promotionReportResponce.Links = promotionLinkDtoList;
                //promotionReportResponce.Coupons = promotionCouponDtoList;
                //responce.data = promotionReportResponce;
                promotionReportResponce.data = list;

                return StatusCode(StatusCodes.Status200OK, promotionReportResponce);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ReturnResponse.ExceptionResponse(ex));
                throw;
            }
        }
    }
}
