using System;

namespace CouponService.Models.Dto
{
    public class CouponReadDto
    {
        public string CouponId { get; set; }
        public string PromotionId { get; set; }
        public string UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
