using CouponService.Abstraction;
using CouponService.Models;
using CouponService.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoutesSecurity;
using System;
using static CouponService.Models.ResponseModel.Response;

namespace CouponService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/")]
    public class PromotionsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PromotionsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpDelete]
        [Route("promotions/{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ReturnResponse.ErrorResponse(CommonMessage.InvalidData, 400));
                }
                _unitOfWork.PromotionRepository.Delete(Obfuscation.Decode(id));
                _unitOfWork.Save();
                return StatusCode(StatusCodes.Status200OK, ReturnResponse.SuccessResponse(CommonMessage.PromotionsDelete, false));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ReturnResponse.ExceptionResponse(ex));
            }
        }

        [HttpDelete]
        [Route("promotions/advertisements/{advertisementId?}")]
        public IActionResult DeletePromotionsFromAdvertisementID([FromQuery] string advertisementId)
        {
            try
            {
                if (string.IsNullOrEmpty(advertisementId))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ReturnResponse.ErrorResponse(CommonMessage.InvalidData, 400));
                }
                Promotion promotion = _unitOfWork.PromotionRepository.Where(x => x.Advertisement_Id == Obfuscation.Decode(advertisementId));
                _unitOfWork.PromotionRepository.Delete(promotion.PromotionId);
                _unitOfWork.Save();
                return StatusCode(StatusCodes.Status200OK, ReturnResponse.SuccessResponse(CommonMessage.PromotionsDelete, false));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ReturnResponse.ExceptionResponse(ex));
            }
        }

        [HttpPost]
        [Route("promotions")]
        public ActionResult Post(Promotion promotion)
        {
            try
            {
                promotion.Advertisement_Id = Obfuscation.Decode(promotion.AdvertisementId);
                promotion.Institution_Id = Obfuscation.Decode(promotion.InstitutionId);
                //required params common in both coupon and links
                if (string.IsNullOrEmpty(promotion.Title) || string.IsNullOrEmpty(promotion.Subtitle) || string.IsNullOrEmpty(promotion.Code) || string.IsNullOrEmpty(promotion.Type.ToString()) || promotion.AdvertisementId == null || promotion.InstitutionId == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ReturnResponse.ErrorResponse(CommonMessage.InvalidData, 400));
                }
                else
                {
                    _unitOfWork.BeginTransaction();
                    _unitOfWork.PromotionRepository.Post(promotion);
                    _unitOfWork.Save();
                    int promotionId = promotion.PromotionId;
                    if (promotion.Type.Equals(PromotionType.coupons))
                    {
                        if (promotion.StartAt == null || promotion.EndAt == null || promotion.UsageLimit == null || promotion.IsSharable == null) // coupon specific required params
                            return StatusCode(StatusCodes.Status400BadRequest, ReturnResponse.ErrorResponse(CommonMessage.InvalidData, 400));
                        else
                        {
                            Coupon coupon = new Coupon() { PromotionId = promotionId, CreatedAt = DateTime.Now };
                            _unitOfWork.CouponRepository.Post(coupon);
                            _unitOfWork.Save();
                        }
                    }
                    else if (promotion.Type.Equals(PromotionType.links)) // links specific required params
                    {
                        if (string.IsNullOrEmpty(promotion.Links.Web) || string.IsNullOrEmpty(promotion.Links.Android) || string.IsNullOrEmpty(promotion.Links.Ios))
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, ReturnResponse.ErrorResponse(CommonMessage.InvalidData, 400));
                        }
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, ReturnResponse.ErrorResponse(CommonMessage.InvalidData, 400));
                    }
                    _unitOfWork.Commit();
                    return StatusCode(StatusCodes.Status200OK, ReturnResponse.SuccessResponse(CommonMessage.PromotionsInsert, true, promotion.PromotionId));
                }
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return StatusCode(StatusCodes.Status400BadRequest, ReturnResponse.ExceptionResponse(ex));
            }
        }

        [HttpPut]
        [Route("promotions/{id}")]
        public ActionResult UpdatePromotion(string id, [FromBody] Promotion promotion)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ReturnResponse.ErrorResponse(CommonMessage.InvalidData, 400));
                }
                var _promotion = _unitOfWork.PromotionRepository.GetById(x => x.PromotionId == Obfuscation.Decode(id), null, x => x.Coupons, x => x.Links);
                if (_promotion is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, ReturnResponse.ErrorResponse(CommonMessage.PromotionsNotFound, 404));
                }
                if (promotion.Type == PromotionType.coupons)
                {
                    _promotion.Advertisement_Id = Obfuscation.Decode(promotion.AdvertisementId);
                    _promotion.Institution_Id = Obfuscation.Decode(promotion.InstitutionId);
                    _promotion.Title = promotion.Title;
                    _promotion.Subtitle = promotion.Subtitle;
                    _promotion.StartAt = promotion.StartAt;
                    _promotion.EndAt = promotion.EndAt;
                    _promotion.Code = promotion.Code;
                    _promotion.IsSharable = promotion.IsSharable;
                    _promotion.UsageLimit = promotion.UsageLimit;
                    _promotion.Type = promotion.Type;
                    _promotion.UpdatedAt = DateTime.Now;
                }
                else if (promotion.Type == PromotionType.links)
                {
                    _promotion.Advertisement_Id = Obfuscation.Decode(promotion.AdvertisementId);
                    _promotion.Institution_Id = Obfuscation.Decode(promotion.InstitutionId);
                    _promotion.Title = promotion.Title;
                    _promotion.Subtitle = promotion.Subtitle;
                    _promotion.Code = promotion.Code;
                    _promotion.Links.Web = promotion.Links.Web;
                    _promotion.Links.Android = promotion.Links.Android;
                    _promotion.Links.Ios = promotion.Links.Ios;
                    _promotion.Type = promotion.Type;
                    _promotion.UpdatedAt = DateTime.Now;

                }
                _unitOfWork.PromotionRepository.Put(_promotion);
                _unitOfWork.Save();

                return StatusCode(StatusCodes.Status200OK, ReturnResponse.SuccessResponse(CommonMessage.PromotionsUpdate, false));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ReturnResponse.ExceptionResponse(ex));
            }
        }
    }
}