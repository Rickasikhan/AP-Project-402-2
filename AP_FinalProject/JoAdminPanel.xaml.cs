using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AP_FinalProject
{
    /// <summary>
    /// Interaction logic for JoAdminPanel.xaml
    /// </summary>
    public partial class JoAdminPanel : Page
    {
        public JoAdminPanel()
        {
            InitializeComponent();
        }

        private void AddDishButton_Click(object sender, RoutedEventArgs e)
        {
            AddDishCanvas.Visibility = Visibility.Visible;
        }

        private void ApplyAddDishButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new MyDbContext())
            {
                if (string.IsNullOrEmpty(DishNameTextBox.Text) ||
                    string.IsNullOrEmpty(DishDiscriptionTextBox.Text) ||
                    string.IsNullOrEmpty(AddDishPriceTextBox.Text) ||
                    string.IsNullOrEmpty(DishAvailibilityTextBox.Text) ||
                    DishTypeComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                double dishprice;
                int availibility;
                if (!double.TryParse(AddDishPriceTextBox.Text.Trim(), out dishprice) ||
                    !int.TryParse(DishAvailibilityTextBox.Text.Trim(), out availibility))
                {
                    MessageBox.Show("Price and availability boxes are not valid.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var newDish = new Dish
                {
                    Name = DishNameTextBox.Text,
                    Description = DishDiscriptionTextBox.Text,
                    Price = dishprice,
                    Availability = availibility,
                    ImageUrl = "Empty.jpg",
                    AverageRating = 0,
                    DishType = DishTypeComboBox.SelectedItem.ToString().Split(':')[1]
                };

                dbContext.Dishes.Add(newDish);
                dbContext.SaveChanges();
            }
            AddDishCanvas.Visibility = Visibility.Collapsed;
        }


        private void CloseAddDishButton_Click(object sender, RoutedEventArgs e)
        {

            AddDishCanvas.Visibility = Visibility.Collapsed;
        }

        private void RemoveDishButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveDishCanvas.Visibility= Visibility.Visible;
        }

        private void RemoveCloseButton_Click(object sender, RoutedEventArgs e)
        {
            
            RemoveDishCanvas.Visibility = Visibility.Visible;
        }

        private void RemoveApplyButton_Click(object sender, RoutedEventArgs e)
        {
            int dishid = -1;
            using (var dbContext = new MyDbContext())
            {
                if (string.IsNullOrEmpty(RemoveDishIdTextBox.Text) == true)
                {
                    MessageBox.Show("Please fill in the field.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    dishid = int.Parse(RemoveDishIdTextBox.Text.Trim());
                }
                catch
                {
                    MessageBox.Show("invalid dish id input", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var dish3 = dbContext.Dishes.FirstOrDefault(d => d.DishId == dishid);
                if (dish3 == null)
                {
                    MessageBox.Show("invalid dish id input", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var comments = dbContext.Comments.Where(c => c.DishId == dishid).ToList();
                dbContext.Comments.RemoveRange(comments);
                dbContext.SaveChanges();
                dbContext.Dishes.Remove(dish3);
                RemoveDishCanvas.Visibility = Visibility.Visible;
            }
        }
    }
}
