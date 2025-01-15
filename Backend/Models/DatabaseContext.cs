using Microsoft.EntityFrameworkCore;
using StarterKit.Models;
using StarterKit.Utils;

namespace StarterKit.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Admin> Admin { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<Event_Attendance> Event_Attendance { get; set; }
        public DbSet<Event> Event { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ensure Admin's UserName is unique
            modelBuilder.Entity<Admin>()
                .HasIndex(a => a.UserName)
                .IsUnique();

            // Ensure User's Email is unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configurations for the new fields
            modelBuilder.Entity<User>()
                .Property(u => u.PasswordResetToken)
                .HasMaxLength(256);

            modelBuilder.Entity<User>()
                .Property(u => u.TokenExpiry);

            // Seed Admin data
            modelBuilder.Entity<Admin>().HasData(
                new Admin { AdminId = 1, Email = "admin1@example.com", UserName = "admin1", Password = EncryptionHelper.EncryptPassword("password") },
                new Admin { AdminId = 2, Email = "admin2@example.com", UserName = "admin2", Password = EncryptionHelper.EncryptPassword("tooeasytooguess") },
                new Admin { AdminId = 3, Email = "admin3@example.com", UserName = "admin3", Password = EncryptionHelper.EncryptPassword("helloworld") },
                new Admin { AdminId = 4, Email = "admin4@example.com", UserName = "admin4", Password = EncryptionHelper.EncryptPassword("Welcome123") },
                new Admin { AdminId = 5, Email = "admin5@example.com", UserName = "admin5", Password = EncryptionHelper.EncryptPassword("Whatisapassword?") }
            );

            // Relationships
            modelBuilder.Entity<User>()
                .HasMany(u => u.Attendances)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Event_Attendances)
                .WithOne(ea => ea.User)
                .HasForeignKey(ea => ea.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Event>()
                .HasMany(e => e.Event_Attendances)
                .WithOne(ea => ea.Event)
                .HasForeignKey(ea => ea.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
