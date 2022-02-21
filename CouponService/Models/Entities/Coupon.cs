using System;
using System.Collections.Generic;

namespace CouponService.Models.Entities
{
    public partial class Coupon
    {
        public Coupon()
        {
            Redemptions = new List<Redemption>();
        }
        public int CouponId { get; set; }

        public int? PromotionId { get; set; }

        public int? UserId { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public virtual Promotion Promotion { get; set; }

        public virtual IList<Redemption> Redemptions { get; set; }
    }
}
