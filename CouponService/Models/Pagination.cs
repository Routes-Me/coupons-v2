using System;

namespace CouponService.Models
{
    public class Pagination
    {
        public int Offset { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public int? Total { get; set; }
        public int? Pages => Total.HasValue ? (int)Math.Ceiling(Total.Value / (double)Limit) : (int?)null;
    }
}
