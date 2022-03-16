using AdvertisementService.Models;
using CouponService.Abstraction;
using CouponService.Models;

namespace CouponService.Repository
{
    public class ReportRepository : GenericRepository<Report>, IReportRepository
    {
        public ReportRepository(CouponContext context) : base(context)
        {
        }
    }
}