using System;

namespace CouponService.Models.Entities
{
    public class Redemption
    {
        public int RedemptionId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? CouponId { get; set; }

        public int? OfficerId { get; set; }

        public virtual Coupon Coupon { get; set; }
    }
}
