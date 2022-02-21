using System;
using System.Collections.Generic;

namespace CouponService.Models.Entities
{
    public partial class Promotion
    {
        public Promotion()
        {
        }
        public int PromotionId { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public int? UsageLimit { get; set; }
        public int? AdvertisementId { get; set; }
        public int? InstitutionId { get; set; }
        public bool? IsSharable { get; set; }
        public string LogoUrl { get; set; }
        public PromotionType Type { get; set; }
        public string Code { get; set; }
        public virtual Coupon Coupons { get; set; }
        public virtual Link Links { get; set; }
    }
    public enum PromotionType
    {
        links,
        coupons
    }

}
