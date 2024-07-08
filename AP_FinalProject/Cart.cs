using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AP_FinalProject
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        public int UserId { get; set; }
        public string PayType {  get; set; }
        public virtual User User { get; set; }
        public virtual List<CartItem> Items { get; set; } = new List<CartItem>();

        public DateTime date { get; set; }
        public string Price { get; set; }

        public Cart() { }
        public void AddToCart(Dish dish, int quantity)
        {
            var existingItem = Items.Find(item => item.Dish.DishId == dish.DishId);
            if (existingItem != null)
            {
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
                Items.Add(new CartItem { Dish = dish, Quantity = quantity, CartId = this.CartId, DishId = dish.DishId});
            }
            dish.Availability -= quantity;
        }

        public void RemoveItem(int dishId)
        {
            var itemToRemove = Items.FirstOrDefault(item => item.Dish.DishId == dishId);
            if (itemToRemove != null)
            {
                var dish = itemToRemove.Dish;
                dish.Availability += itemToRemove.Quantity;
                Items.Remove(itemToRemove);
            }
        }

        public double CalculateTotalPrice()
        {
            double totalPrice = 0;
            foreach (var item in Items)
            {
                totalPrice += item.Dish.Price * item.Quantity;
            }
            return totalPrice;
        }

        public void ClearCart()
        {
            Items.Clear();
        }
    }

    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public virtual Cart Cart { get; set; }
        public int DishId { get; set; }
        public virtual Dish Dish { get; set; }
        public int Quantity { get; set; }

        public CartItem() { }
    }

    
}
