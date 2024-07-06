using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_FinalProject
{
    public class Restaurant
    {
        [Key]
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Type { get; set; }
        public double Rating { get; set; }
        public string Address { get; set; }

        public Restaurant()
        {
        }

        public Restaurant(int restaurantId, string name, string city, string type, double rating, string address)
        {
            RestaurantId = restaurantId;
            Name = name;
            City = city;
            Type = type;
            Rating = rating;
            Address = address;
        }
    }


}
