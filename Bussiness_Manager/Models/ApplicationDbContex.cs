using Microsoft.EntityFrameworkCore;

namespace Bussiness_Manager.Models
{
    public class ApplicationDbContex : DbContext
    {
        public ApplicationDbContex(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Member to ShopDetails relationship
            modelBuilder.Entity<ShopDetail>()
                .HasOne(sd => sd.Members)
                .WithMany(m => m.ShopDetails)
                .HasForeignKey(sd => sd.memberId)
                .OnDelete(DeleteBehavior.Cascade);

            // Member to Product relationship
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Members)
                .WithMany(m => m.Products)
                .HasForeignKey(p => p.memberId)
                .OnDelete(DeleteBehavior.Cascade);

            // Member to Customer relationship
            modelBuilder.Entity<Customer>()
                .HasOne(p => p.Members)
                .WithMany(m => m.Customers)
                .HasForeignKey(p => p.memberId)
                .OnDelete(DeleteBehavior.Cascade);

            // Member to saleInvoice relationship
            modelBuilder.Entity<saleInvoice>()
                .HasOne(p => p.Members)
                .WithMany(m => m.saleInvoices)
                .HasForeignKey(p => p.memberId)
                .OnDelete(DeleteBehavior.Cascade);

            // Member to Transactions relationship
            modelBuilder.Entity<Transactions>()
                .HasOne(p => p.Members)
                .WithMany(m => m.Transactions)
                .HasForeignKey(p => p.memberId)
                .OnDelete(DeleteBehavior.Cascade);

            // ShopDetails to Product relationship
            modelBuilder.Entity<Product>()
                .HasOne(p => p.ShopDetail)
                .WithMany(sd => sd.Products)
                .HasForeignKey(p => p.shopId)
                .OnDelete(DeleteBehavior.NoAction);

            // ShopDetails to Customer relationship
            modelBuilder.Entity<Customer>()
                .HasOne(p => p.ShopDetail)
                .WithMany(sd => sd.Customers)
                .HasForeignKey(p => p.shopId)
                .OnDelete(DeleteBehavior.Restrict);

            // ShopDetails to saleInvoice relationship
            modelBuilder.Entity<saleInvoice>()
                .HasOne(p => p.ShopDetail)
                .WithMany(sd => sd.saleInvoices)
                .HasForeignKey(p => p.shopId)
                .OnDelete(DeleteBehavior.Restrict);

            // ShopDetails to Transactions relationship
            modelBuilder.Entity<Transactions>()
                .HasOne(p => p.ShopDetail)
                .WithMany(sd => sd.Transactions)
                .HasForeignKey(p => p.shopId)
                .OnDelete(DeleteBehavior.Restrict);

            // Product to saleInvoiceDetail relationship
            modelBuilder.Entity<saleInvoiceDetail>()
                .HasOne(p => p.Product)
                .WithMany(sd => sd.saleInvoiceDetails)
                .HasForeignKey(p => p.productId)
                .OnDelete(DeleteBehavior.Restrict);

            // Customer to saleInvoice relationship
            modelBuilder.Entity<saleInvoice>()
                .HasOne(p => p.Customer)
                .WithMany(sd => sd.saleInvoices)
                .HasForeignKey(p => p.customerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Customer to Transactions relationship
            modelBuilder.Entity<Transactions>()
                .HasOne(p => p.Customer)
                .WithMany(sd => sd.Transactions)
                .HasForeignKey(p => p.customerId)
                .OnDelete(DeleteBehavior.Restrict);

            // saleInvoice to saleInvoiceDetail relationship
            modelBuilder.Entity<saleInvoiceDetail>()
                .HasOne(p => p.SaleInvoice)
                .WithMany(sd => sd.saleInvoiceDetails)
                .HasForeignKey(p => p.saleId)
                .OnDelete(DeleteBehavior.Restrict);

            // saleInvoice to Transaction relationship
            modelBuilder.Entity<Transactions>()
                .HasOne(p => p.saleInvoice)
                .WithMany(sd => sd.Transactions)
                .HasForeignKey(p => p.saleId)
                .OnDelete(DeleteBehavior.Restrict);

        }




        public DbSet<Members> members { get; set; }
        public DbSet<ShopDetail> shopDetails { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Customer> customers { get; set; }
        public DbSet<saleInvoice> saleInvoices { get; set; }
        public DbSet<saleInvoiceDetail> saleInvoiceDetails { get; set; }
        public DbSet<Unit> units { get; set; }
        public DbSet<Transactions> transactions { get; set; }
    }   
}
