namespace CouponService.Abstraction
{
    public interface IUnitOfWork
    {
        IPromotionRepository PromotionRepository { get; }
        ILinkRepository LinkRepository { get; }
        ICouponRepository CouponRepository{ get; }
        IReportRepository ReportRepository{ get; }
        void BeginTransaction();
        void Commit();
        void Rollback();

        void Save();
    }
}
