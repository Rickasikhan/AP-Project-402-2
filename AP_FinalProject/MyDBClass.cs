using System.Data.Entity;

namespace AP_FinalProject
{
    public class MyDbContext : DbContext
    {
        public MyDbContext() : base("Data Source=ASUS\\SQLEXPRESS;Initial Catalog=AP-ProjectDatabase;Integrated Security=True;")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .HasOptional(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId);

            modelBuilder.Entity<Comment>()
                .HasRequired(c => c.Dish)
                .WithMany(d => d.Comments)
                .HasForeignKey(c => c.DishId);

            modelBuilder.Entity<Rating>()
                .HasRequired(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Rating>()
                .HasRequired(r => r.Dish)
                .WithMany(d => d.Ratings)
                .HasForeignKey(r => r.DishId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
