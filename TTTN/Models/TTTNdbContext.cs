namespace TTTN.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TTTNdbContext : DbContext
    {
        public TTTNdbContext()
            : base("name=TTTNdbContext")
        {
        }

        public virtual DbSet<C_category> C_category { get; set; }
        public virtual DbSet<C_contact> C_contact { get; set; }
        public virtual DbSet<C_menu> C_menu { get; set; }
        public virtual DbSet<C_order> C_order { get; set; }
        public virtual DbSet<C_orderdetail> C_orderdetail { get; set; }
        public virtual DbSet<C_post> C_post { get; set; }
        public virtual DbSet<C_product> C_product { get; set; }
        public virtual DbSet<C_slider> C_slider { get; set; }
        public virtual DbSet<C_topic> C_topic { get; set; }
        public virtual DbSet<C_user> C_user { get; set; }
        public virtual DbSet<C_link> C_link { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<C_category>()
                .Property(e => e.category_createdat)
                .HasPrecision(6);

            modelBuilder.Entity<C_category>()
                .Property(e => e.category_updatedat)
                .HasPrecision(6);

            modelBuilder.Entity<C_category>()
                .HasMany(e => e.C_product)
                .WithRequired(e => e.C_category)
                .HasForeignKey(e => e.product_catid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<C_contact>()
                .Property(e => e.contact_createdat)
                .HasPrecision(6);

            modelBuilder.Entity<C_contact>()
                .Property(e => e.contact_updatedat)
                .HasPrecision(6);

            modelBuilder.Entity<C_menu>()
                .Property(e => e.menu_createdat)
                .HasPrecision(6);

            modelBuilder.Entity<C_menu>()
                .Property(e => e.menu_updatedat)
                .HasPrecision(6);

            modelBuilder.Entity<C_order>()
                .Property(e => e.order_updatedat)
                .HasPrecision(6);

            modelBuilder.Entity<C_order>()
                .HasMany(e => e.C_orderdetail)
                .WithRequired(e => e.C_order)
                .HasForeignKey(e => e.orderdetail_orderid)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<C_post>()
                .Property(e => e.post_createdat)
                .HasPrecision(6);

            modelBuilder.Entity<C_post>()
                .Property(e => e.post_updatedat)
                .HasPrecision(6);

            modelBuilder.Entity<C_product>()
                .Property(e => e.product_createdat)
                .HasPrecision(6);

            modelBuilder.Entity<C_product>()
                .Property(e => e.product_updatedat)
                .HasPrecision(6);

            modelBuilder.Entity<C_slider>()
                .Property(e => e.slider_createdat)
                .HasPrecision(6);

            modelBuilder.Entity<C_slider>()
                .Property(e => e.slider_updatedat)
                .HasPrecision(6);

            modelBuilder.Entity<C_topic>()
                .Property(e => e.topic_createdat)
                .HasPrecision(6);

            modelBuilder.Entity<C_topic>()
                .Property(e => e.topic_updatedat)
                .HasPrecision(6);

            modelBuilder.Entity<C_user>()
                .Property(e => e.user_createdat)
                .HasPrecision(6);

            modelBuilder.Entity<C_user>()
                .Property(e => e.user_updatedat)
                .HasPrecision(6);

            modelBuilder.Entity<C_link>()
                .Property(e => e.slug)
                .IsUnicode(false);

            modelBuilder.Entity<C_link>()
                .Property(e => e.type)
                .IsUnicode(false);
        }
    }
}
