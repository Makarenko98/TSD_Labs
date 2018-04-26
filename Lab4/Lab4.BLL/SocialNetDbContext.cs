using Lab4.BLL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Lab4.BLL
{
    public class SocialNetDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChatUser> ChatUsers { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<UserFriend> UserFriends { get; set; }
        public DbSet<UserPhoto> UserPhotos { get; set; }

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
            //modelBuilder.Entity<FriendRequest>()
            //    .HasIndex(fr => new { fr.FromUserId, fr.ToUserId }).IsUnique();
            modelBuilder.Entity<UserFriend>()
                .HasIndex(fr => new { fr.UserId, fr.FriendId }).IsUnique();
            modelBuilder.Entity<UserPhoto>()
                .Property(uf => uf.LoadTime).HasDefaultValueSql("getutcdate()");

            modelBuilder.Entity<UserPhoto>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserPhotos)
                .HasForeignKey(up => up.UserId);

            modelBuilder.Entity<UserFriend>()
                .HasOne(uf => uf.User)
                .WithMany(u => u.UserFriends)
                .HasForeignKey(uf => uf.UserId);

            modelBuilder.Entity<UserFriend>()
                .HasOne(uf => uf.Friend)
                .WithMany(u => u.Followers)
                .HasForeignKey(pt => pt.FriendId);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) {
                relationship.DeleteBehavior = DeleteBehavior.Restrict; // set on delete no action
            }
        }
    }
}
