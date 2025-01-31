using System.Data.Entity;
using TaskManagementSystem.Models.DatabaseModels;

namespace TaskManagementSystem.Context
{
    public class DBContext : DbContext
    {
        public DBContext() : base("name=DefaultDatabaseConnectionString")
        {
        }

        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }
        public virtual DbSet<TaskStatuses> TaskStatuses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Username as Unique Constraint
            modelBuilder.Entity<Users>()
                .Property(u => u.Username)
                .IsRequired() // Ensures Username is not null
                .HasMaxLength(50); // Optional: String length limit

            modelBuilder.Entity<Users>()
                .HasKey(u => u.ID); // Set primary key for Users table
            modelBuilder.Entity<Users>()
                .HasIndex(u => u.Username)
                .IsUnique()
                .HasName("UK_Username"); // Explicitly name the unique constraint

            // Configure RoleName as Unique Constraint
            modelBuilder.Entity<Roles>()
                .Property(r => r.RoleName)
                .IsRequired() // Ensures RoleName is not null
                .HasMaxLength(50); // Optional: String length limit

            modelBuilder.Entity<Roles>()
                .HasKey(r => r.RoleID); // Set primary key for Roles table
            modelBuilder.Entity<Roles>()
                .HasIndex(r => r.RoleName)
                .IsUnique()
                .HasName("UK_RoleName"); // Explicitly name the unique constraint
        }

    }
}
