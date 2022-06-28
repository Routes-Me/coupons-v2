using System;

namespace CouponService.Models.Dto
{
    public class AdvertisementReadDto
    {
        public string AdvertisementId { get; set; }
        public string ResourceName { get; set; }
        public string InstitutionId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string MediaId { get; set; }
        public int? TintColor { get; set; }
        public int? InvertedTintColor { get; set; }
    }
}
