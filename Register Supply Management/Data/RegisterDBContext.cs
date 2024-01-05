using Microsoft.EntityFrameworkCore;
using Register_Supply_Management.Model.Data;
using Register_Supply_Management.Utilities.Handlers;

namespace Register_Supply_Management.Data
{
    public class RegisterDBContext : DbContext
    {
        public RegisterDBContext(DbContextOptions<RegisterDBContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin", CreatedAt = DateTime.Now, ModifiedAt = DateTime.Now },
                new Role { Id = 2, Name = "Manager", CreatedAt = DateTime.Now, ModifiedAt = DateTime.Now },
                new Role { Id = 3, Name = "Vendor", CreatedAt = DateTime.Now, ModifiedAt = DateTime.Now }
                );

            modelBuilder.Entity<Account>().HasData(
                new Account { Id = 1, Name = "Admin", Username = "Admin", Password = Hashing.HashPassword("Admin"), RoleId = 1, IsDeleted = false, CreatedAt = DateTime.Now, ModifiedAt = DateTime.Now },
                new Account { Id = 2, Name = "Manager", Username = "Manager", Password = Hashing.HashPassword("Manager"), RoleId = 2, IsDeleted = false, CreatedAt = DateTime.Now, ModifiedAt = DateTime.Now }
                );

            // Set Unique
            modelBuilder.Entity<Vendor>().HasIndex(ve => new
            {
                ve.Email,
                ve.PhoneNumber
            }).IsUnique();

            modelBuilder.Entity<Account>().HasIndex(acc => new
            {
                acc.Username
            }).IsUnique();

            // Set Relationship

            // Role - Account (One to Many)
            modelBuilder.Entity<Account>()
                .HasOne(acc => acc.Role)
                .WithMany(role => role.Accounts)
                .HasForeignKey(acc => acc.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Vendor - Account (One to One)
            modelBuilder.Entity<Account>()
                .HasOne(acc => acc.Vendor)
                .WithOne(vendor => vendor.Account)
                .HasForeignKey<Vendor>(ve => ve.AccountId)
                .IsRequired(false);

            // Vendor - Project (One to Many)
            modelBuilder.Entity<Project>()
                .HasOne(pro => pro.Vendor)
                .WithMany(ve => ve.Projects)
                .HasForeignKey(pro => pro.VendorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
