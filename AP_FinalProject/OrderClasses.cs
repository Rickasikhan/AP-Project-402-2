using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AP_FinalProject
{
    public class Dish
    {
        [Key]
        public int DishId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Availability { get; set; }
        public string ImageUrl { get; set; }
        public double AverageRating { get; set; }
        public string DishType { get; set; } // Added DishType

        public virtual ICollection<Comment> Comments { get; set; } // Navigation property
        public virtual ICollection<Rating> Ratings { get; set; } // Navigation property

        public Dish()
        {
            Comments = new List<Comment>();
            Ratings = new List<Rating>();
        }
    }
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }
        public int UserId { get; set; }
        public int DishId { get; set; }
        public double RatingValue { get; set; }
        public DateTime RatedAt { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("DishId")]
        public virtual Dish Dish { get; set; }

        public Rating()
        {
            RatedAt = DateTime.Now;
        }
    }
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public string Text { get; set; }
        public string Username { get; set; }
        public string Dishname { get; set; }
        public DateTime CreatedAt { get; set; }

        public int? ParentCommentId { get; set; }

        public int DishId { get; set; }

        [ForeignKey("ParentCommentId")]
        public virtual Comment ParentComment { get; set; }
        public virtual ICollection<Comment> Replies { get; set; }

        [ForeignKey("DishId")]
        public virtual Dish Dish { get; set; }

        public Comment()
        {
            CreatedAt = DateTime.Now;
            Replies = new List<Comment>();
        }
    }
    public class Complaint
    {
        [Key]
        public int ComplaintId { get; set; }
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("RestaurantId")]
        public virtual Restaurant Restaurant { get; set; }

        public Complaint()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
