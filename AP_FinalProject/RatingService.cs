using AP_FinalProject;
using System;
using System.Linq;

public class RatingService
{
    private readonly MyDbContext _context;

    public RatingService(MyDbContext context)
    {
        _context = context;
    }

    public void AddOrUpdateRating(int userId, int dishId, float ratingValue)
    {

        var rating = _context.Ratings.FirstOrDefault(r => r.UserId == userId && r.DishId == dishId);
        if (rating == null)
        {
            rating = new Rating
            {
                UserId = userId,
                DishId = dishId,
                RatingValue = ratingValue,
                RatedAt = DateTime.Now
            };
            _context.Ratings.Add(rating);
        }
        else
        {
            rating.RatingValue = ratingValue;
            rating.RatedAt = DateTime.Now;
            _context.Entry(rating).State = System.Data.Entity.EntityState.Modified;
        }
        _context.SaveChanges();

        UpdateDishAverageRating(dishId);
    }

    public void DeleteRating(int userId, int dishId)
    {
        var rating = _context.Ratings.FirstOrDefault(r => r.UserId == userId && r.DishId == dishId);
        if (rating != null)
        {
            _context.Ratings.Remove(rating);
            _context.SaveChanges();
        }
        UpdateDishAverageRating(dishId);
    }

    private void UpdateDishAverageRating(int dishId)
    {
        var dish = _context.Dishes.Find(dishId);
        if (dish != null)
        {
            dish.AverageRating = _context.Ratings
                .Where(r => r.DishId == dishId)
                .Average(r => r.RatingValue);
            _context.Entry(dish).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
