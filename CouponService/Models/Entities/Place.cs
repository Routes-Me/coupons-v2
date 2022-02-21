using System.Collections.Generic;

namespace CouponService.Models.Entities
{
    public partial class Place
    {
        public Place()
        {
            PromotionsPlaces = new List<PromotionsPlace>();
        }
        public int PlaceId { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public string Name { get; set; }
        public virtual IList<PromotionsPlace> PromotionsPlaces { get; set; }
    }
}
