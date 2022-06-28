using CouponService.Abstraction;
using CouponService.Models;
using CouponService.Models.Entities;

namespace CouponService.Repository
{
    public class LinkRepository : GenericRepository<Link>, ILinkRepository
    {
        public LinkRepository(CouponContext context) : base(context)
        {
        }
    }
}
