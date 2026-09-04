using Domain.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Post> Post { get; set; }
        public DbSet<ChatMessage> ChatMessage { get; set; }
        public DbSet<Like> Like { get; set; }
        public DbSet<Match> Match { get; set; }
        public DbSet<Report> Report { get; set; }
        public DbSet<Activity> Activity { get; set; }
        public DbSet<UserBlocks> UserBlocks { get; set; }
        public DbSet<UserDevice> UserDevices { get; set; }
        public DbSet<Conversation> Conversation { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Match>()
            .HasOne(m => m.Creator)  // Match has one User1
            //.WithMany(u => u.Matches)  // ApplicationUser has many Matches
            .WithMany()
            .HasForeignKey(m => m.CreatorId)  // Foreign key in Match for User1
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Match>()
            .HasOne(m => m.Acceptor)  // Match has one User2
            .WithMany()  // ApplicationUser has no navigation property for Matches of User2
            .HasForeignKey(m => m.AcceptorId)  // Foreign key in Match for User2
            .OnDelete(DeleteBehavior.Restrict);


            //builder.Entity<ChatMessage>()
            //.HasOne(m => m.Sender)
            //.WithMany()
            //.HasForeignKey(m => m.SenderId)
            //.OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<ChatMessage>()
            //.HasOne(m => m.Receiver)
            //.WithMany()
            //.HasForeignKey(m => m.ReceiverId)
            //.OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<ChatMessage>()
            .HasOne(m => m.Conversation)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ConversationId)
            .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Report>()
            .HasOne(r => r.Reporter)
            .WithMany(u => u.Reports)
            .HasForeignKey(r => r.ReporterId)
            .OnDelete(DeleteBehavior.Restrict); // prevent cascade issues

            builder.Entity<Report>()
            .HasOne(r => r.ReportedUser)
            .WithMany()
            .HasForeignKey(r => r.ReportedUserId)
            .OnDelete(DeleteBehavior.Restrict); // optional

            builder.Entity<Post>()
            .HasMany(p => p.Reports)
            .WithOne(r => r.ReportedPost)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Post>()
            .HasMany(p => p.Likes)
            .WithOne(l => l.Post)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserBlocks>()
            .HasOne(b => b.BlockerUser)
            .WithMany(u => u.BlockedUsers)
            .HasForeignKey(b => b.BlockerUserId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserBlocks>()
            .HasOne(b => b.BlockedUser)
            .WithMany(u => u.BlockedByUsers)
            .HasForeignKey(b => b.BlockedUserId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserBlocks>()
            .HasIndex(b => new { b.BlockerUserId, b.BlockedUserId })
            .IsUnique(); // Prevent duplicate blocks

            builder.Entity<UserDevice>()
            .HasOne(u => u.User)
            .WithMany(u => u.Devices)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Conversation>()
            .HasOne(c => c.User1)
            .WithMany()
            .HasForeignKey(c => c.User1Id)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Conversation>()
            .HasOne(c => c.User2)
            .WithMany()
            .HasForeignKey(c => c.User2Id)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Conversation>()
            .HasIndex(c => new { c.User1Id, c.User2Id })
            .IsUnique();

            builder.Entity<RefreshToken>()
            .HasOne(r => r.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FriendRequest>()
            .HasOne(fr => fr.Sender)
            .WithMany()
            .HasForeignKey(fr => fr.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FriendRequest>()
            .HasOne(fr => fr.Receiver)
            .WithMany()
            .HasForeignKey(fr => fr.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FriendRequest>()
            .HasIndex(fr => new { fr.SenderId, fr.ReceiverId })
            .IsUnique();

            builder.Entity<ApplicationUser>().HasQueryFilter(u => !u.IsBanned && !u.IsDeleted);
            builder.Entity<Post>().HasQueryFilter(p => !p.User.IsBanned && !p.User.IsDeleted);
        }
    }
}
