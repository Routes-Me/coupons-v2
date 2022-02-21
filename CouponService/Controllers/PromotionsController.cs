using CouponService.Abstraction;
using CouponService.Models;
using CouponService.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        [Route("promotions")]
        public ActionResult Post(Promotion promotion)
        {
            try
            {

                promotion.Code = !string.IsNullOrEmpty(promotion.Code) ? (promotion.Code.Length > 5 ? promotion.Code.Substring(0, 5) : promotion.Code) : null;

                promotion.StartAt = string.IsNullOrEmpty(promotion.StartAt.ToString()) ? DateTime.Now : promotion.StartAt;
                promotion.EndAt = string.IsNullOrEmpty(promotion.EndAt.ToString()) ? DateTime.Now.AddMonths(1) : promotion.EndAt;

                if (!string.IsNullOrEmpty(promotion.Type.ToString()))
                {
                    if (promotion.Type.Equals(PromotionType.coupons))
                    {
                        promotion.UsageLimit = string.IsNullOrEmpty(promotion.UsageLimit.ToString()) ? 1000 : promotion.UsageLimit;
                    }
                }
                else
                    return StatusCode(StatusCodes.Status404NotFound, ReturnResponse.ErrorResponse(CommonMessage.PromotionsNotFound, 404));

                _unitOfWork.BeginTransaction();

                _unitOfWork.PromotionRepository.Post(promotion);
                _unitOfWork.Save();

                int promotionId = promotion.PromotionId;
                if (promotion.Type.Equals(PromotionType.coupons))
                {
                    Coupon coupon = new Coupon() { PromotionId = promotionId, CreatedAt = DateTime.Now };
                    _unitOfWork.CouponRepository.Post(coupon);
                    _unitOfWork.Save();

                }

                _unitOfWork.Commit();
                return StatusCode(StatusCodes.Status200OK, ReturnResponse.SuccessResponse(CommonMessage.PromotionsInsert, true));
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return StatusCode(StatusCodes.Status400BadRequest, ReturnResponse.ExceptionResponse(ex));
            }
        }
    }
}
