using Microsoft.EntityFrameworkCore;
using PhotoContest.Models;

namespace PhotoContest.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Contest> Contests { get; set; } 
        public DbSet<PhotoPost> PhotoPosts { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Role> Roles { get; set; }        
        public DbSet<ContestUser> ContestUser { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
      
            modelBuilder
                .Entity<Rating>()
                .HasOne(rating => rating.PhotoPost)
                .WithMany(PhotoPost => PhotoPost.Ratings)
                .HasForeignKey(rating => rating.PhotoPostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<Rating>()
                .HasOne(rating => rating.User)
                .WithMany(user => user.Ratings)
                .HasForeignKey(rating => rating.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Seed();
        }
    }
}
