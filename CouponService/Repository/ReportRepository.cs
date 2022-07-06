using CouponService.Abstraction;
using CouponService.Models;

namespace CouponService.Repository
{
    public class ReportRepository : GenericRepository<Reports>, IReportRepository
    {
        public ReportRepository(CouponContext context) : base(context)
        {
        }
    }
}