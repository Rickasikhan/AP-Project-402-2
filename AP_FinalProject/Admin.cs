using System.Collections.Generic;
using System.Linq;

namespace AP_FinalProject
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public Admin() { }

        public void AddOrUpdateRestaurant(MyDbContext context, Restaurant restaurant)
        {
            var existingRestaurant = context.Restaurants.FirstOrDefault(r => r.RestaurantId == restaurant.RestaurantId);
            if (existingRestaurant != null)
            {
                // Update existing restaurant
                /*existingRestaurant.Name = restaurant.Name;
                existingRestaurant.Address = restaurant.Address;
                existingRestaurant.City = restaurant.City;
                existingRestaurant.State = restaurant.State;
                existingRestaurant.ZipCode = restaurant.ZipCode;
                existingRestaurant.PhoneNumber = restaurant.PhoneNumber;
                existingRestaurant.Email = restaurant.Email;
                existingRestaurant.Rating = restaurant.Rating;*/
            }
            else
            {
                context.Restaurants.Add(restaurant);
            }
            context.SaveChanges();
        }
        public void RemoveRestaurant(MyDbContext context, int restaurantId)
        {
            var restaurant = context.Restaurants.Find(restaurantId);
            if (restaurant != null)
            {
                context.Restaurants.Remove(restaurant);
                context.SaveChanges();
            }
        }

        public List<Restaurant> SearchRestaurantByName(MyDbContext context, string name)
        {
            return context.Restaurants.Where(r => r.Name.Contains(name)).ToList();
        }
        public List<Restaurant> SearchRestaurantByCity(MyDbContext context, string city)
        {
            return context.Restaurants.Where(r => r.City.Contains(city)).ToList();
        }
        public List<User> GetAllUsers(MyDbContext context)
        {
            return context.Users.ToList();
        }
        public void ChangeUserRole(MyDbContext context, int userId, string newRole)
        {
            var user = context.Users.Find(userId);
            if (user != null)
            {
                user.Service = newRole;
                context.SaveChanges();
            }
        }
        public List<Complaint> GetAllComplaints(MyDbContext context)
        {
            return context.Complaints.ToList();
        }

        public void ResolveComplaint(MyDbContext context, int complaintId)
        {
            var complaint = context.Complaints.Find(complaintId);
            if (complaint != null)
            {
                context.Complaints.Remove(complaint);
                context.SaveChanges();
            }
        }
    }

    public class JoAdmin
    {
        public int JoAdminId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public JoAdmin() { }
    }
}
