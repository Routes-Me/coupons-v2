using CouponService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdvertisementService.Models
{
    public partial class CouponContext : DbContext
    {
        public CouponContext()
        {
        }
        public CouponContext(DbContextOptions<CouponContext> options) : base(options)
        {
        }
        public virtual DbSet<Coupon> Coupons{ get; set; }
        public virtual DbSet<Link> Links{ get; set; }
        public virtual DbSet<Place> Places{ get; set; }
        public virtual DbSet<Promotion> Promotions{ get; set; }
        public virtual DbSet<PromotionsPlace> PromotionsPlaces{ get; set; }
        public virtual DbSet<Redemption> Redemptions{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coupon>(entity =>
            {
                entity.HasKey(e => e.CouponId)
                    .HasName("PRIMARY");

                entity.ToTable("coupons");

                entity.HasIndex(e => e.PromotionId)
                    .HasName("promotion_id");

                entity.Property(e => e.CouponId).HasColumnName("coupon_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.PromotionId).HasColumnName("promotion_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => (System.Collections.Generic.IEnumerable<Coupon>)p.Coupons)
                    .HasForeignKey(d => d.PromotionId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("coupons_ibfk_1");
            });

            modelBuilder.Entity<Link>(entity =>
            {
                entity.HasKey(e => e.LinkId)
                    .HasName("PRIMARY");

                entity.ToTable("links");

                entity.HasIndex(e => e.PromotionId)
                    .HasName("promotion_id");

                entity.Property(e => e.LinkId).HasColumnName("link_id");

                entity.Property(e => e.Android)
                    .HasColumnName("android")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.Ios)
                    .HasColumnName("ios")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.PromotionId).HasColumnName("promotion_id");

                entity.Property(e => e.Web)
                    .HasColumnName("web")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.HasOne(d => d.Promotion)
                    .WithMany(p => (System.Collections.Generic.IEnumerable<Link>)p.Links)
                    .HasForeignKey(d => d.PromotionId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("links_ibfk_1");
            });

            modelBuilder.Entity<Place>(entity =>
            {
                entity.HasKey(e => e.PlaceId)
                    .HasName("PRIMARY");

                entity.ToTable("places");

                entity.Property(e => e.PlaceId).HasColumnName("place_id");

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude")
                    .HasColumnType("decimal(10,8)");

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude")
                    .HasColumnType("decimal(11,8)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Promotion>(entity =>
            {
                entity.HasKey(e => e.PromotionId)
                    .HasName("PRIMARY");

                entity.ToTable("promotions");

                entity.Property(e => e.PromotionId).HasColumnName("promotion_id");

                entity.Property(e => e.AdvertisementId).HasColumnName("advertisement_id");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasColumnType("varchar(5)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.EndAt)
                    .HasColumnName("end_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.InstitutionId).HasColumnName("institution_id");

                entity.Property(e => e.IsSharable).HasColumnName("is_sharable");

                entity.Property(e => e.LogoUrl)
                    .HasColumnName("logo_url")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.StartAt)
                    .HasColumnName("start_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.Subtitle)
                    .HasColumnName("subtitle")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasColumnType("enum('places','links','coupons')")
                    .HasCharSet("latin1")
                    .HasCollation("latin1_swedish_ci");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.UsageLimit).HasColumnName("usage_limit");

            });

            //modelBuilder.Entity<PromotionsPlace>(entity =>
            //{
            //    entity.HasKey(e => new { e.PromotionId, e.PlaceId })
            //        .HasName("PRIMARY")
            //        .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            //    entity.ToTable("promotions_places");

            //    entity.HasIndex(e => e.PlaceId)
            //        .HasName("place_id");

            //    entity.Property(e => e.PromotionId).HasColumnName("promotion_id");

            //    entity.Property(e => e.PlaceId).HasColumnName("place_id");

            //    entity.HasOne(d => d.Place)
            //        .WithMany(p => p.PromotionsPlaces)
            //        .HasForeignKey(d => d.PlaceId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("promotions_places_ibfk_1");

            //    entity.HasOne(d => d.Promotion)
            //        .WithMany(p => p.PromotionsPlaces)
            //        .HasForeignKey(d => d.PromotionId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("promotions_places_ibfk_2");
            //});

            modelBuilder.Entity<Redemption>(entity =>
            {
                entity.HasKey(e => e.RedemptionId)
                    .HasName("PRIMARY");

                entity.ToTable("redemptions");

                entity.HasIndex(e => e.CouponId)
                    .HasName("coupon_id");

                entity.Property(e => e.RedemptionId).HasColumnName("redemption_id");

                entity.Property(e => e.CouponId).HasColumnName("coupon_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.Property(e => e.OfficerId).HasColumnName("officer_id");

                entity.HasOne(d => d.Coupon)
                    .WithMany(p => p.Redemptions)
                    .HasForeignKey(d => d.CouponId)
                    .HasConstraintName("redemptions_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
