using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
        [NotMapped]
        public string AdvertisementId { get; set; }
        public int? Advertisement_Id { get; set; }
        [NotMapped]
        public string InstitutionId { get; set; }
        public int Institution_Id { get; set; }
        public bool? IsSharable { get; set; }
        public string LogoUrl { get; set; }
        public PromotionType? Type { get; set; }
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
