using System;
using System.Collections.Generic;

namespace CouponService.Models.Dto
{
    public class PromotionCouponDto
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string code { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public int? UsageLimit { get; set; }
        public bool? isSharable { get; set; }
        public string AdvertisementId { get; set; }
        public string InstitutionId { get; set; }
        public string type { get ; set; }
    }

    public class PromotionLinkDto
    {
        public PromotionLinkDto()
        {
            link = new LinkReadDto();
        }

        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string code { get; set; }
        public virtual LinkReadDto link { get; set; }
        public string type { get; set; }
        public string AdvertisementId { get; set; }
        public string InstitutionId { get; set; }
    }

    public class PromotionReportResponce
    {
        public List<PromotionLinkDto> Links { get; set; }
        public List<PromotionCouponDto> Coupons { get; set; }
    }

}
