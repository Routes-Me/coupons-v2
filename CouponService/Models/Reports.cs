using CouponService.Models.Entities;
using System;

namespace CouponService.Models
{
    public partial class Report
    {
        public string PromotionId { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public int AdvertisementId { get; set; }
        public string InstitutionId { get; set; }
        public string LogoUrl { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public int? UsageLimit { get; set; }
        public string Coupon { get; set; }
        public string Link { get; set; }
        public bool? IsSharable { get; set; }
    }
}