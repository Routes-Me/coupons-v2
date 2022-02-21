using CouponService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdvertisementService.Models
{
    public class CouponContext : DbContext
    {
        public CouponContext()
        {
        }
        public CouponContext(DbContextOptions<CouponContext> options) : base(options)
        {
        }
        public virtual DbSet<Coupon> Coupons{ get; set; }
        public virtual DbSet<Link> Links{ get; set; }
        public virtual DbSet<Place> Places{ get; set; }
        public virtual DbSet<Promotion> Promotions{ get; set; }
        public virtual DbSet<PromotionsPlace> PromotionsPlaces{ get; set; }
        public virtual DbSet<Redemption> Redemptions{ get; set; }
    }
}
