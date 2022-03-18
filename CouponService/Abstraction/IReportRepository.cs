using CouponService.Models;
using CouponService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CouponService.Abstraction
{
    public interface IReportRepository : IGenericRepository<Report>
    {
        //List<Promotion> GetReports(List<string> advertisementId, params Expression<Func<Reports, object>>[] include);
    }
}
