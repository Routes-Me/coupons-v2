namespace CouponService.Abstraction
{
    public interface IUnitOfWork
    {
        IPromotionRepository PromotionRepository { get; }
        ILinkRepository LinkRepository { get; }
        ICouponRepository CouponRepository{ get; }
        void BeginTransaction();
        void Commit();
        void Rollback();

        void Save();
    }
}
