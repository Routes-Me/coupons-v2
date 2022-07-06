namespace CouponService.Models.Entities
{
    public class Link
    {
        public int LinkId { get; set; }

        public int? PromotionId { get; set; }

        public string Web { get; set; }

        public string Ios { get; set; }

        public string Android { get; set; }

        public virtual Promotion Promotion { get; set; }
    }
}
