using AdvertisementService.Models;
using CouponService.Abstraction;
using CouponService.Models.Entities;

namespace CouponService.Repository
{
    public class CouponRepository:GenericRepository<Coupon>, ICouponRepository
    {
        public CouponRepository(CouponContext context):base(context)
        {

        }
    }
}
