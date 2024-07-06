using System;
using System.Collections.Generic;
using System.Linq;

namespace AP_FinalProject
{
    public static class Cart
    {
        private static List<CartItem> items = new List<CartItem>();

        public static void AddToCart(Dish dish, int quantity)
        {
            // Check if the dish is already in the cart
            var existingItem = items.Find(item => item.Dish.DishId == dish.DishId);
            if (existingItem != null)
            {
                // Check if adding this quantity exceeds availability
                if (existingItem.Quantity + quantity > dish.Availability)
                {
                    throw new InvalidOperationException($"Only {dish.Availability - existingItem.Quantity} more can be added to cart for '{dish.Name}'.");
                }
                existingItem.Quantity += quantity;
            }
            else
            {
                if (quantity > dish.Availability)
                {
                    throw new InvalidOperationException($"Only {dish.Availability} available for '{dish.Name}'.");
                }
                items.Add(new CartItem { Dish = dish, Quantity = quantity });
            }
            dish.Availability -= quantity;
        }

        public static List<CartItem> GetCartItems()
        {
            return items;
        }

        public static double CalculateTotalPrice()
        {
            double totalPrice = 0;
            foreach (var item in items)
            {
                totalPrice += item.Dish.Price * item.Quantity;
            }
            return totalPrice;
        }

        public static void ClearCart()
        {
            items.Clear();
        }
    }

    public class CartItem
    {
        public Dish Dish { get; set; }
        public int Quantity { get; set; }
    }
}
