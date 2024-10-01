//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection.Metadata;
using DailyPlanManager.Models;
using Microsoft.EntityFrameworkCore;


namespace DailyPlanManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<User> User { get; set; }
        public DbSet<DailyPlan> DailyPlan { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(dp => dp.DailyPlans)
                .WithOne(u => u.User)
                .HasForeignKey(dp => dp.User_Id)
                .IsRequired();
        }
    }
}