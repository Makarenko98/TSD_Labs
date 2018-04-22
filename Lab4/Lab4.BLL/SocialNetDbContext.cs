using Lab4.BLL.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Lab4.BLL
{
    public class SocialNetDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChatUser> ChatUsers { get; set; }

        public SocialNetDbContext() : base()
        { }

        public SocialNetDbContext(string connectionString)
            : base(new DbContextOptionsBuilder<SocialNetDbContext>()
                .UseSqlServer(connectionString)
                .Options)
        { }

        public SocialNetDbContext(DbContextOptions options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ChatUser>()
                .HasKey(cu => new { cu.ChatId, cu.UserId });
            modelBuilder.Entity<Message>()
                .Property(m => m.Time).HasDefaultValueSql("getutcdate()");
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Login).IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email).IsUnique();
        }
    }
}
