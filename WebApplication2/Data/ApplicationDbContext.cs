using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Discount> Discounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
       
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Subscriptions)
                .WithOne(s => s.Client)
                .HasForeignKey(s => s.ClientId);

            
            modelBuilder.Entity<Subscription>()
                .HasMany(s => s.Payments)
                .WithOne(p => p.Subscription)
                .HasForeignKey(p => p.SubscriptionId);

           
            modelBuilder.Entity<Subscription>()
                .HasMany<Discount>(s => s.Discounts)
                .WithOne(d => d.Subscription)
                .HasForeignKey(d => d.SubscriptionId);

           modelBuilder.Entity<Subscription>()
               .HasMany<Payment>( s =>s.Payments)
               .WithOne(d => d.Subscription)
               .HasForeignKey(d => d.SubscriptionId);
            base.OnModelCreating(modelBuilder);
        }



    }
}