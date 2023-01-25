using CouponService.Abstraction;
using CouponService.Models;
using CouponService.Models.Entities;

namespace CouponService.Repository
{
    public class PromotionRepository : GenericRepository<Promotion>, IPromotionRepository
    {
        public PromotionRepository(CouponContext context) : base(context)
        {
        }
    }
}
