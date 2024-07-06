using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace AP_FinalProject
{
    public partial class Jo : Page
    {
        private MyDbContext context;
        public string Username;

        private int currentDishId;

        public Jo()
        {
            InitializeComponent();

            context = new MyDbContext();

            LoadDishes("Salad");
            SaladButton.Click += (sender, e) => LoadDishes("Salad");
            MainDishButton.Click += (sender, e) => LoadDishes("Main Dish");
            DessertButton.Click += (sender, e) => LoadDishes("Dessert");
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

                Button commentsButton = new Button
                {
                    Content = "View Comments",
                    Tag = dish.DishId
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
                    Tag = dish.DishId
                };
                rateButton.Click += (sender, e) =>
                {
                    int dishId = (int)rateButton.Tag;
                    float ratingValue = PromptUserForRating();

                    using (var context = new MyDbContext())
                    {
                        var ratingService = new RatingService(context);
                        var user = context.Users.FirstOrDefault(u => u.Username == this.Username);
                        ratingService.AddOrUpdateRating(user.UserId, dishId, ratingValue);

                        var updatedDish = context.Dishes.Find(dishId);
                        ratingTextBlock.Text = "Rating: " + updatedDish.AverageRating.ToString();
                    }
                };
                dishPanel.Children.Add(rateButton);

                dishBorder.Child = dishPanel;
                SaladStackPanel.Children.Add(dishBorder);
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
                    rating = 5.0f; // Default to 5 if out of range
                }
            }
            else
            {
                MessageBox.Show("Invalid rating value.");
                rating = 5.0f; // Default to 5 if invalid input
            }
            return rating;
        }

        private void DisplayComments(int dishId)
        {
            CommentStackPanel.Children.Clear();

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
                Tag = comment.CommentId
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
                    Tag = parentCommentId
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

                    // Display updated comments after adding a reply
                    DisplayComments(comment.DishId);
                };
                replyPanel.Children.Add(addReplyButton);

                // Add the reply panel
                parentPanel.Children.Add(replyPanel);
            };
            commentPanel.Children.Add(replyButton);

            // Add the comment panel
            parentPanel.Children.Add(commentPanel);

            // Recursively add replies
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

                if (user == null)
                {
                    MessageBox.Show("User not found.");
                    return;
                }

                if (Jo == null)
                {
                    MessageBox.Show("Restaurant not found.");
                    return;
                }

                var complaint = new Complaint
                {
                    UserId = user.UserId,
                    RestaurantId = Jo.RestaurantId,
                    Subject = SubjectTextBox.Text,
                    Description = DescriptionTextBox.Text,
                    CreatedAt = DateTime.Now 
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
            SubmitComplaint();
            ComplaintCanvas.Visibility= Visibility.Collapsed;
        }

        private void CloseComplainButton_Click(object sender, RoutedEventArgs e)
        {
            ComplaintCanvas.Visibility = Visibility.Collapsed;
        }
    }
}
