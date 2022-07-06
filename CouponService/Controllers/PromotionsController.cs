using CouponService.Abstraction;
using CouponService.Extensions;
using CouponService.Models;
using CouponService.Models.Base;
using CouponService.Models.Dto;
using CouponService.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using RoutesSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using static CouponService.Models.ResponseModel.Response;

namespace CouponService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    public class PromotionsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;
        private readonly Dependencies _dependencies;

        public PromotionsController(IUnitOfWork unitOfWork, IOptions<AppSettings> appSettings, IOptions<Dependencies> dependencies)
        {
            _unitOfWork = unitOfWork;
            _appSettings = appSettings.Value;
            _dependencies = dependencies.Value;
        }

        [HttpDelete]
        [Route("{id}")]
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
        [Route("advertisements/{advertisementId?}")]
        public IActionResult DeletePromotionsFromAdvertisementId([FromQuery] string advertisementId)
        {
            try
            {
                if (string.IsNullOrEmpty(advertisementId))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ReturnResponse.ErrorResponse(CommonMessage.InvalidData, 400));
                }
                var promotions = _unitOfWork.PromotionRepository.Get(null, x => x.Advertisement_Id == Obfuscation.Decode(advertisementId), null);
                foreach(var promotion in promotions)
                {
                    _unitOfWork.PromotionRepository.Remove(promotion);
                    _unitOfWork.Save();
                }
                return StatusCode(StatusCodes.Status200OK, ReturnResponse.SuccessResponse(CommonMessage.PromotionsDelete, false));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ReturnResponse.ExceptionResponse(ex));
            }
        }

        [HttpPost]
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
                    
                    if (promotion.Type.Equals(PromotionType.Coupons))
                    {
                        if (promotion.StartAt == null || promotion.EndAt == null || promotion.UsageLimit == null || promotion.IsSharable == null) // coupon specific required params
                            return StatusCode(StatusCodes.Status400BadRequest, ReturnResponse.ErrorResponse(CommonMessage.InvalidData, 400));
                        else
                        {
                            var coupon = new Coupon() { PromotionId = promotion.PromotionId, CreatedAt = DateTime.Now };
                            _unitOfWork.CouponRepository.Post(coupon);
                            _unitOfWork.Save();
                        }
                    }
                    else if (promotion.Type.Equals(PromotionType.Links)) // links specific required params
                    {
                        if (string.IsNullOrEmpty(value: promotion.Link.Web) || string.IsNullOrEmpty(promotion.Link.Android) || string.IsNullOrEmpty(promotion.Link.Ios))
                        {
                            return StatusCode(StatusCodes.Status400BadRequest, ReturnResponse.ErrorResponse(CommonMessage.InvalidData, 400));
                        }
                        else
                        {
                            var link = new Link
                            {
                                Web = promotion.Link.Web,
                                Ios = promotion.Link.Android,
                                Android = promotion.Link.Ios,
                                PromotionId = promotion.PromotionId

                            };
                            _unitOfWork.LinkRepository.Post(link);
                            _unitOfWork.Save();
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
        [Route("{id}")]
        public ActionResult UpdatePromotion(string id, [FromBody] Promotion promotion)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ReturnResponse.ErrorResponse(CommonMessage.InvalidData, 400));
                }
                var _promotion = _unitOfWork.PromotionRepository.GetById(x => x.PromotionId == Obfuscation.Decode(id), null, x => x.Coupon, x => x.Link);
                if (_promotion is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, ReturnResponse.ErrorResponse(CommonMessage.PromotionsNotFound, 404));
                }
                if (_promotion.Type == PromotionType.Coupons)
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
                else if (_promotion.Type == PromotionType.Links)
                {
                    _promotion.Advertisement_Id = Obfuscation.Decode(promotion.AdvertisementId);
                    _promotion.Institution_Id = Obfuscation.Decode(promotion.InstitutionId);
                    _promotion.Title = promotion.Title;
                    _promotion.Subtitle = promotion.Subtitle;
                    _promotion.Code = promotion.Code;
                    _promotion.Link.Web = promotion.Link.Web;
                    _promotion.Link.Android = promotion.Link.Android;
                    _promotion.Link.Ios = promotion.Link.Ios;
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





        [HttpGet]
        [Route("{promotionId?}")]
        public IActionResult GetById(string promotionId, string include, [FromQuery] Pagination pageInfo)
        {
            var response = new GetResponseById<Promotion>();

            try
            {
                var promotionReadDto = new PromotionReadDto();
                var promotion = _unitOfWork.PromotionRepository.GetById(x => x.PromotionId == Obfuscation.Decode(promotionId));

                if (promotion == null)
                    throw new Exception(CommonMessage.PromotionsNotFound);

                //promotionReadDto = _mapper.Map<PromotionReadDto>(promotion);

                promotionReadDto.PromotionId = Obfuscation.Encode(Convert.ToInt32(promotion.PromotionId));
                promotionReadDto.Advertisement_Id = Obfuscation.Encode(Convert.ToInt32(promotion.Advertisement_Id));
                promotionReadDto.Institution_Id = Obfuscation.Encode(Convert.ToInt32(promotion.Institution_Id));

                dynamic includeData = new JObject();
                if (!string.IsNullOrEmpty(include) && promotionReadDto != null)
                {
                    var includeArr = include.Split(',');
                    if (includeArr.Length > 0)
                    {
                        foreach (var item in includeArr)
                        {
                            if (item.ToLower() == "institution" || item.ToLower() == "institutions")
                            {
                                includeData.institution = ApiExtensions.GetByIdInstitutionsIncludedData(promotionReadDto, _appSettings.Host + _dependencies.InstitutionUrl);
                            }
                            else if (item.ToLower() == "advertisement" || item.ToLower() == "advertisements")
                            {
                                includeData.advertisements = ApiExtensions.GetAdvertisementsByIdIncludedData(promotionReadDto, _appSettings.Host + _dependencies.AdvertisementsUrl);
                            }

                        }
                    }
                }

                if (((JContainer)includeData).Count == 0)
                    includeData = null;

                response.Status = true;
                response.Message = CommonMessage.PromotionsRetrived;
                response.Data = promotion;
                response.Included = includeData;
                response.Code = StatusCodes.Status200OK;

                return StatusCode(response.Code, response);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ReturnResponse.ExceptionResponse(ex));
            }
        }

        [HttpGet]
        public IActionResult Get(string include, [FromQuery] Pagination pageInfo)
        {
            var response = new GetResponse<Promotion>();

            try
            {
                var promotionReadDto = new List<PromotionReadDto>();
                var promotions = _unitOfWork.PromotionRepository.Get(pageInfo, null, x => x.OrderBy(x => x.PromotionId), x => x.Coupon, x => x.Link).ToList();


                foreach (var promotion in promotionReadDto)
                {
                    promotion.PromotionId = Obfuscation.Encode(Convert.ToInt32(promotion.PromotionId));
                    promotion.Advertisement_Id = Obfuscation.Encode(Convert.ToInt32(promotion.Advertisement_Id));
                    promotion.Institution_Id = Obfuscation.Encode(Convert.ToInt32(promotion.Institution_Id));
                }

                dynamic includeData = new JObject();
                if (!string.IsNullOrEmpty(include) && promotionReadDto.Count > 0)
                {
                    var includeArr = include.Split(',');
                    if (includeArr.Length > 0)
                    {
                        foreach (var item in includeArr)
                        {
                            if (item.ToLower() == "institution" || item.ToLower() == "institutions")
                            {
                                includeData.institution = ApiExtensions.GetInstitutionsIncludedData(promotionReadDto, _appSettings.Host + _dependencies.InstitutionUrl);
                            }
                            else if (item.ToLower() == "advertisement" || item.ToLower() == "advertisements")
                            {
                                includeData.advertisements = ApiExtensions.GetAdvertisementsIncludedData(promotionReadDto, _appSettings.Host + _dependencies.AdvertisementsUrl);
                            }

                        }
                    }
                }

                if (((JContainer)includeData).Count == 0)
                    includeData = null;

                response.Status = true;
                response.Message = CommonMessage.PromotionsRetrived;
                response.Pagination = pageInfo;
                response.Data = promotions;
                response.Included = includeData;
                response.Code = StatusCodes.Status200OK;

                return StatusCode(response.Code, response);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ReturnResponse.ExceptionResponse(e));
            }
        }

    }
}