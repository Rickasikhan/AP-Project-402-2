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
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<JoAdmin> JoAdmins { get; set; } // Add DbSet for JoAdmin

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

            modelBuilder.Entity<Complaint>()
                .HasRequired(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Complaint>()
                .HasRequired(c => c.Restaurant)
                .WithMany()
                .HasForeignKey(c => c.RestaurantId);

            modelBuilder.Entity<Cart>()
                .HasRequired(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<CartItem>()
                .HasRequired(ci => ci.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(ci => ci.CartId);

            modelBuilder.Entity<CartItem>()
                .HasRequired(ci => ci.Dish)
                .WithMany()
                .HasForeignKey(ci => ci.DishId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
