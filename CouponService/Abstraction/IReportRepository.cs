using CouponService.Models;

namespace CouponService.Abstraction
{
    public interface IReportRepository : IGenericRepository<Reports>
    {
        //List<Promotion> GetReports(List<string> advertisementId, params Expression<Func<Reports, object>>[] include);
    }
}
