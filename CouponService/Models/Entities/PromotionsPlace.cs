namespace CouponService.Models.Entities
{
    public class PromotionsPlace
    {
        public int PromotionsPlaceId { get; set; }
        public int PromotionId { get; set; }
        public int PlaceId { get; set; }

        public Place Place { get; set; }
    }
}
