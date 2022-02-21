using AdvertisementService.Models;
using CouponService.Abstraction;
using System;

namespace CouponService.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly CouponContext _context;
        private bool disposed = false;
        public UnitOfWork(CouponContext context)
        {
            _context = context;
            if (PromotionRepository is null)
            {
                PromotionRepository = new PromotionRepository(_context);
            }
            if (LinkRepository is null)
            {
                LinkRepository = new LinkRepository(_context);
            }if (CouponRepository is null)
            {
                CouponRepository = new CouponRepository(_context);
            }
        }

        public ILinkRepository LinkRepository { get; }
        public IPromotionRepository PromotionRepository { get; private set; }
        public ICouponRepository CouponRepository{ get; private set; }
        public void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _context.Database.CommitTransaction();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Rollback()
        {
            _context.Database.RollbackTransaction();
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}
