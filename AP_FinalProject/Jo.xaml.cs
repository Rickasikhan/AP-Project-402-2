using System;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace AP_FinalProject
{
    public partial class Jo : Page
    {
        private MyDbContext context;
        private Cart cart;
        private int currentDishId;
        public string Username;

        public Jo()
        {
            InitializeComponent();

            context = new MyDbContext();
            cart = new Cart();

            LoadDishes("Salad");
            LoadDishes("Main Dish");
            LoadDishes("Dessert");
            SaladButton.Click += (sender, e) => LoadDishes("Salad");
            MainDishButton.Click += (sender, e) => LoadDishes("Main Dish");
            DessertButton.Click += (sender, e) => LoadDishes("Dessert");

            UpdateRestaurantAverageRating();
        }

        private void LoadDishes(string dishType)
        {
            SaladStackPanel.Children.Clear();

            var dishes = context.Dishes
                                .Where(d => d.DishType == dishType)
                                .ToList();

            foreach (var dish in dishes)
            {
                Border dishBorder = new Border
                {
                    BorderThickness = new Thickness(1),
                    BorderBrush = System.Windows.Media.Brushes.Black,
                    Margin = new Thickness(5),
                    Padding = new Thickness(5)
                };

                StackPanel dishPanel = new StackPanel();

                Image dishImage = new Image
                {
                    Source = new BitmapImage(new Uri(dish.ImageUrl, UriKind.RelativeOrAbsolute)),
                    Height = 100,
                    Width = 100
                };
                dishPanel.Children.Add(dishImage);

                TextBlock nameTextBlock = new TextBlock
                {
                    Text = dish.Name,
                    FontWeight = FontWeights.Bold
                };
                dishPanel.Children.Add(nameTextBlock);

                TextBlock descriptionTextBlock = new TextBlock
                {
                    Text = dish.Description,
                    TextWrapping = TextWrapping.Wrap
                };
                dishPanel.Children.Add(descriptionTextBlock);

                TextBlock priceTextBlock = new TextBlock
                {
                    Text = $"Price: ${dish.Price}"
                };
                dishPanel.Children.Add(priceTextBlock);

                TextBlock availabilityTextBlock = new TextBlock
                {
                    Text = $"Available: {dish.Availability}"
                };
                dishPanel.Children.Add(availabilityTextBlock);

                TextBlock ratingTextBlock = new TextBlock
                {
                    Text = $"Rating: {dish.AverageRating}"
                };
                dishPanel.Children.Add(ratingTextBlock);

                Button addToCartButton = new Button
                {
                    Content = "Add to Cart",
                    Tag = dish.DishId,
                    Style = (Style)FindResource("SoftButtonStyle")
                };
                addToCartButton.Click += (sender, e) =>
                {
                    int dishId = (int)((Button)sender).Tag;
                    var selectedDish = context.Dishes.Find(dishId);
                    if (selectedDish != null)
                    {
                        try
                        {
                            cart.AddToCart(selectedDish, 1);
                        }
                        catch (InvalidOperationException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                };
                dishPanel.Children.Add(addToCartButton);

                dishBorder.Child = dishPanel;

                Button commentsButton = new Button
                {
                    Content = "View Comments",
                    Tag = dish.DishId,
                    Style = (Style)FindResource("SoftButtonStyle")
                };
                commentsButton.Click += (sender, e) =>
                {
                    int dishId = (int)commentsButton.Tag;
                    currentDishId = dishId;
                    DisplayComments(dishId);
                    CommentSection.Visibility = Visibility.Visible;
                };
                dishPanel.Children.Add(commentsButton);

                Button rateButton = new Button
                {
                    Content = "Rate",
                    Tag = dish.DishId,
                    Style = (Style)FindResource("SoftButtonStyle")
                };
                rateButton.Click += (sender, e) =>
                {
                    int dishId = (int)rateButton.Tag;
                    float ratingValue = PromptUserForRating();

                    var ratingService = new RatingService(context);
                    var user = context.Users.FirstOrDefault(u => u.Username == this.Username);
                    ratingService.AddOrUpdateRating(user.UserId, dishId, ratingValue);

                    var updatedDish = context.Dishes.Find(dishId);
                    ratingTextBlock.Text = "Rating: " + updatedDish.AverageRating.ToString();
                    UpdateRestaurantAverageRating();
                };
                dishPanel.Children.Add(rateButton);

                dishBorder.Child = dishPanel;
                SaladStackPanel.Children.Add(dishBorder);
                UpdateRestaurantAverageRating();
            }
        }

        private float PromptUserForRating()
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Enter rating value (1-5):", "Rate Dish", "5");
            if (float.TryParse(input, out float rating))
            {
                if (rating < 1 || rating > 5)
                {
                    MessageBox.Show("Rating must be between 1 and 5.");
                    rating = 5.0f;
                }
            }
            else
            {
                MessageBox.Show("Invalid rating value.");
                rating = 5.0f;
            }
            return rating;
        }

        private void DisplayComments(int dishId)
        {
            CommentStackPanel.Children.Clear();
            //CommentStackPanel.Height = 400;
            var comments = context.Comments
                                  .Include(c => c.Replies)
                                  .Where(c => c.DishId == dishId && c.ParentCommentId == null)
                                  .ToList();

            foreach (var comment in comments)
            {
                AddCommentToUI(comment, CommentStackPanel, 0);
            }
        }

        private void AddCommentToUI(Comment comment, StackPanel parentPanel, int indentLevel)
        {
            StackPanel commentPanel = new StackPanel
            {
                Margin = new Thickness(20 * indentLevel, 5, 5, 5)
            };

            TextBlock usernameTextBlock = new TextBlock
            {
                Text = comment.Username,
                FontWeight = FontWeights.Bold
            };
            commentPanel.Children.Add(usernameTextBlock);

            TextBlock commentTextBlock = new TextBlock
            {
                Text = comment.Text,
                TextWrapping = TextWrapping.Wrap
            };
            commentPanel.Children.Add(commentTextBlock);

            TextBlock createdAtTextBlock = new TextBlock
            {
                Text = comment.CreatedAt.ToString(),
                FontStyle = FontStyles.Italic,
                FontSize = 10
            };
            commentPanel.Children.Add(createdAtTextBlock);

            Button replyButton = new Button
            {
                Content = "Reply",
                Tag = comment.CommentId,
                Style = (Style)FindResource("SoftButtonStyle")
            };
            replyButton.Click += (sender, e) =>
            {
                int parentCommentId = (int)replyButton.Tag;
                StackPanel replyPanel = new StackPanel();

                TextBox replyTextBox = new TextBox
                {
                    Width = 200,
                    Height = 50,
                    Margin = new Thickness(5)
                };
                replyPanel.Children.Add(replyTextBox);

                Button addReplyButton = new Button
                {
                    Content = "Add Reply",
                    Tag = parentCommentId,
                    Style = (Style)FindResource("SoftButtonStyle")
                };
                addReplyButton.Click += (replySender, replyE) =>
                {
                    var replyComment = new Comment
                    {
                        Text = replyTextBox.Text,
                        Username = this.Username,
                        DishId = comment.DishId,
                        ParentCommentId = parentCommentId,
                        CreatedAt = DateTime.Now
                    };
                    using (var dbContext = new MyDbContext())
                    {
                        dbContext.Comments.Add(replyComment);
                        dbContext.SaveChanges();
                    }

                    DisplayComments(comment.DishId);
                };
                replyPanel.Children.Add(addReplyButton);
                parentPanel.Children.Add(replyPanel);
            };
            commentPanel.Children.Add(replyButton);
            parentPanel.Children.Add(commentPanel);
            foreach (var reply in comment.Replies)
            {
                AddCommentToUI(reply, parentPanel, indentLevel + 1);
            }
        }

        private void AddCommentButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NewCommentTextBox.Text))
            {
                MessageBox.Show("Please enter a comment.");
                return;
            }

            var newComment = new Comment
            {
                Text = NewCommentTextBox.Text,
                Username = this.Username,
                DishId = currentDishId,
                CreatedAt = DateTime.Now
            };
            using (var dbContext = new MyDbContext())
            {
                dbContext.Comments.Add(newComment);
                dbContext.SaveChanges();
            }

            NewCommentTextBox.Text = string.Empty;

            DisplayComments(currentDishId);
        }

        private void CloseCommentsButton_Click(object sender, RoutedEventArgs e)
        {
            CommentSection.Visibility = Visibility.Collapsed;
        }

        private void ComplainButton_Click(object sender, RoutedEventArgs e)
        {
            ComplaintCanvas.Visibility = Visibility.Visible;
        }

        private void SubmitComplaint()
        {
            using (var context = new MyDbContext())
            {
                User user = context.Users.FirstOrDefault(u => u.Username == Username);
                Restaurant Jo = context.Restaurants.FirstOrDefault(r => r.Name == "Jo");
                var complaint = new Complaint
                {
                    UserId = user.UserId,
                    RestaurantId = Jo.RestaurantId,
                    Subject = SubjectTextBox.Text,
                    Description = DescriptionTextBox.Text,
                    CreatedAt = DateTime.Now,
                    IsCheckedOut = false
                };

                context.Complaints.Add(complaint);
                context.SaveChanges();
            }

            MessageBox.Show("Complaint submitted successfully.");
            SubjectTextBox.Clear();
            DescriptionTextBox.Clear();
        }

        private void SubmitComplaintButton_Click(object sender, RoutedEventArgs e)
        {
            if (SubjectTextBox.Text == string.Empty || DescriptionTextBox.Text == string.Empty)
            {
                MessageBox.Show("Please fill in both the subject and description.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            SubmitComplaint();
            ComplaintCanvas.Visibility = Visibility.Collapsed;
        }

        private void UpdateRestaurantAverageRating()
        {
            var restaurant = context.Restaurants.FirstOrDefault(r => r.Name == "Jo");
            var dishes = context.Dishes.ToList();
            double totalRating = 0;
            int ratedDishesCount = 0;

            foreach (var dish in dishes)
            {
                if (dish.AverageRating != 0)
                {
                    totalRating += dish.AverageRating;
                    ratedDishesCount++;
                }
            }

            if (ratedDishesCount > 0)
            {
                restaurant.Rating = totalRating / ratedDishesCount;
            }
            else
            {
                restaurant.Rating = 0;
            }

            context.SaveChanges();
            AverageRatingTextBlock.Text = $"Average Rating: {restaurant.Rating:F2}";
        }

        private void CloseComplainButton_Click(object sender, RoutedEventArgs e)
        {
            ComplaintCanvas.Visibility = Visibility.Collapsed;
        }

        private void BasketButton_Click(object sender, RoutedEventArgs e)
        {
            CartStackPanel.Children.Clear();

            foreach (var item in cart.Items)
            {
                StackPanel itemPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(10) };

                TextBlock itemName = new TextBlock
                {
                    Text = item.Dish.Name,
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Width = 200
                };

                TextBlock itemPrice = new TextBlock
                {
                    Text = item.Dish.Price.ToString("C"),
                    FontSize = 16,
                    Width = 100
                };

                TextBlock itemQuantity = new TextBlock
                {
                    Text = $"x{item.Quantity}",
                    FontSize = 16,
                    Width = 50
                };

                Button removeButton = new Button
                {
                    Content = "Remove",
                    Tag = item.DishId,
                    Style = (Style)FindResource("SoftButtonStyle")
                };
                removeButton.Click += (senderr, args) =>
                {
                    int dishIdToRemove = (int)((Button)senderr).Tag;
                    var dishToRemove = context.Dishes.Find(dishIdToRemove);
                    if (dishToRemove != null)
                    {
                        var cartItemToRemove = cart.Items.FirstOrDefault(cartItem => cartItem.DishId == dishIdToRemove);
                        if (cartItemToRemove != null)
                        {
                            dishToRemove.Availability += cartItemToRemove.Quantity;
                            cart.RemoveItem(dishIdToRemove);
                            //context.SaveChanges();
                        }
                    }
                };


                itemPanel.Children.Add(itemName);
                itemPanel.Children.Add(itemPrice);
                itemPanel.Children.Add(itemQuantity);
                itemPanel.Children.Add(removeButton);

                CartStackPanel.Children.Add(itemPanel);
            }

            TotalPriceTextBlock.Text = $"Total Price: {cart.CalculateTotalPrice():C}";

            CartSection.Visibility = Visibility.Visible;
        }

        private void CloseCartButton_Click(object sender, RoutedEventArgs e)
        {
            CartSection.Visibility = Visibility.Collapsed;
        }

        private void ClearCartButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in cart.Items)
            {
                var dish = context.Dishes.Find(item.DishId);
                if (dish != null)
                {
                    dish.Availability += item.Quantity;
                }
            }
            cart.ClearCart();
            BasketButton_Click(sender, e);
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            PaymentOptionsCanvas.Visibility = Visibility.Visible;
        }
        private void OfflinePaymentButton_Click(object sender, RoutedEventArgs e)
        {
            cart.date = DateTime.Now;
            cart.PayType = "Offline";
            MessageBox.Show("Payment validated successfully.");
            User user1 = context.Users.FirstOrDefault(u => u.Username == Username);
            cart.UserId = user1.UserId;
            SaveCartToDatabase();
            PaymentOptionsCanvas.Visibility = Visibility.Collapsed;
            cart.ClearCart();
        }

        private void OnlinePaymentButton_Click(object sender, RoutedEventArgs e)
        {
            cart.date = DateTime.Now;
            cart.PayType = "Online";
            string totalPrice = TotalPriceTextBlock.Text.Split(':')[1];
            var user = context.Users.FirstOrDefault(u => u.Username == Username);
            string recipientEmail = user.Email;
            string paymentSubject = "Pending Order";
            SendVerificationEmail(recipientEmail, paymentSubject, totalPrice);
            User user1 = context.Users.FirstOrDefault(u => u.Username == Username);
            cart.UserId = user1.UserId;
            SaveCartToDatabase();
            cart.ClearCart();
            PaymentOptionsCanvas.Visibility = Visibility.Collapsed;
        }

        private void SaveCartToDatabase()
        {
            context.Carts.Add(cart);
            foreach (var item in cart.Items)
            {
                context.CartItems.Add(item);
            }
            cart.Price = TotalPriceTextBlock.Text.Split(':')[1];
            context.SaveChanges();
        }
        private void SendVerificationEmail(string recipientEmail, string paymentSubject, string totalPrice)
        {
            Random random = new Random(DateTime.Now.Millisecond);
            string senderEmail = "kasrakhalaj44@gmail.com";
            string senderPassword = "murb dztj glho owmb";
            string pass = $"{random.Next(100001, 999999).ToString()}";
            string subject = paymentSubject; // Use the provided payment subject
            string body = $"Thank you for your order! Your total amount is {totalPrice:C}. " +
                          $"Please use the following verification code to complete your payment: {pass}";

            SendEmail(senderEmail, senderPassword, recipientEmail, subject, body);
        }
        private void SendEmail(string senderEmail, string senderPassword, string recipientEmail, string subject, string body)
        {
            using (var smtpClient = new SmtpClient("smtp.gmail.com"))
            {
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                smtpClient.EnableSsl = true;
                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(senderEmail);
                    mailMessage.To.Add(new MailAddress(recipientEmail));
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;

                    smtpClient.Send(mailMessage);
                }
            }
        }
        private void SaladButton_Click(object sender, RoutedEventArgs e)
        {
            LoadDishes("Salad");
        }

    }
}
