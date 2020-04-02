using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Entity;

namespace Twitter.Context
{
    public class TwitterContext  : DbContext
    {
        public TwitterContext(DbContextOptions<TwitterContext> options) : base(options)
        {
        }
        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"server=.;database=TwitterDB;Trusted_Connection=True;");
            optionsBuilder.UseLazyLoadingProxies();
        }
        */

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Like>()
                .HasKey(c => new { c.tweetId,c.userId});
            modelBuilder.Entity<Retweet>()
              .HasKey(c => new { c.tweetId, c.userId });

            modelBuilder.Entity<Following>()
              .HasKey(c => new { c.followedId, c.followingId });


            modelBuilder.Entity<Following>()
                .HasOne(c => c.followed)
                .WithMany(p=>p.followeds)
                .HasForeignKey(pc => pc.followedId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Following>()
                .HasOne(c => c.following)
                .WithMany(p => p.followings)
                .HasForeignKey(pc => pc.followingId);

            base.OnModelCreating(modelBuilder);
        }
        
        public DbSet<User> users { get; set; }
        public DbSet<Tweet> tweets { get; set; }
        public DbSet<Retweet> retweets { get; set; }
        public DbSet<Like> likes { get; set; }

        //public DbSet<Following> followings { get; set; }
    }
}
