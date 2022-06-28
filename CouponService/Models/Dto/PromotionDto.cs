﻿using System;

namespace CouponService.Models.Dto
{
    public class PromotionCouponDto
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Code { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public int? UsageLimit { get; set; }
        public bool? IsSharable { get; set; }
        public string AdvertisementId { get; set; }
        public string InstitutionId { get; set; }
        public string Type { get; set; }
    }

    public class PromotionLinkDto
    {
        public PromotionLinkDto()
        {
            Link = new LinkReadDto();
        }

        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Code { get; set; }
        public virtual LinkReadDto Link { get; set; }
        public string Type { get; set; }
        public string AdvertisementId { get; set; }
        public string InstitutionId { get; set; }
    }
    public class PromotionReadDto
    {
        public string PromotionId { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public int? UsageLimit { get; set; }
        public string Advertisement_Id { get; set; }
        public string Institution_Id { get; set; }
        public bool? IsSharable { get; set; }
        public string LogoUrl { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
    }
    public class PromotionReportResponse
    {
        public dynamic Data { get; set; }
    }

}
