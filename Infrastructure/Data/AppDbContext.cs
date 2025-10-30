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
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Post> Post { get; set; }
        public DbSet<ChatMessage> ChatMessage { get; set; }
        public DbSet<Like> Like { get; set; }
        public DbSet<Match> Match { get; set; }
        public DbSet<Report> Report { get; set; }
        public DbSet<Activity> Activity { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Match>()
            .HasOne(m => m.Creator)  // Match has one User1
            .WithMany(u => u.Matches)  // ApplicationUser has many Matches
            .HasForeignKey(m => m.CreatorId)  // Foreign key in Match for User1
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Match>()
            .HasOne(m => m.Acceptor)  // Match has one User2
            .WithMany()  // ApplicationUser has no navigation property for Matches of User2
            .HasForeignKey(m => m.AcceptorId)  // Foreign key in Match for User2
            .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<ChatMessage>()
            .HasOne(m => m.Sender)
            .WithMany()
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ChatMessage>()
            .HasOne(m => m.Receiver)
            .WithMany()
            .HasForeignKey(m => m.ReceiverId)
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
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Post>()
            .HasMany(p => p.Likes)
            .WithOne(l => l.Post)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUser>().HasQueryFilter(u => !u.IsBanned && !u.IsDeleted);
            builder.Entity<Post>().HasQueryFilter(p => !p.User.IsBanned && !p.User.IsDeleted);
        }
    }
}
