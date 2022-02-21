using AdvertisementService.Models;
using CouponService.Abstraction;
using CouponService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CouponService.Repository
{
    public class PromotionRepository:GenericRepository<Promotion>,IPromotionRepository
    {
        public PromotionRepository(CouponContext context):base(context)
        {
        }
    }
}
