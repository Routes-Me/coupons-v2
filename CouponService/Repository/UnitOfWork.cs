using CouponService.Abstraction;
using CouponService.Models;
using System;

namespace CouponService.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly CouponContext _context;
        public ILinkRepository LinkRepository { get; }
        public IPromotionRepository PromotionRepository { get; }
        public ICouponRepository CouponRepository { get; }
        public IReportRepository ReportRepository { get; }
        private bool _disposed = false;
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
            }
            if (CouponRepository is null)
            {
                CouponRepository = new CouponRepository(_context);
            }
            if (ReportRepository is null)
            {
                ReportRepository = new ReportRepository(_context);
            }
        }


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
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }
    }
}
