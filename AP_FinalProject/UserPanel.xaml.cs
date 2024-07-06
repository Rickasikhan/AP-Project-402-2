using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
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
    /// Interaction logic for UserPanel.xaml
    /// </summary>
    public partial class UserPanel : Page
    {
        private List<Restaurant> restaurants;

        public string UserName { get; set; }
        //private List<string> _genders = new List<string> { "Male", "Female", "Other" };

        public UserPanel(string _UserName)
        {
            this.UserName = _UserName;
            try
            {
                //RestaurantsListBox.ItemsSource = restaurants;
            }
            catch (Exception)
            {
                ErrorTB.Visibility = Visibility.Collapsed;
            }
            InitializeComponent();
            LoadRestaurants();
            LoadCities();
            MinRatingSlider.Value = 0; 
            MinRatingTextBlock.Text = $"Min Rating: {MinRatingSlider.Value}";
            //EditGenderComboBox.ItemsSource = _genders;
            using (var dbContext = new MyDbContext())
            {
                var user = dbContext.Users.FirstOrDefault(u => u.Username == UserName);
                LoggedinAsTB.Text = "Logged in as " + user.Username;
                ServiceMedalTB.Text = user.Service;
                if (ServiceMedalTB.Text == "Bronze")
                {
                    ServiceMedalTB.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCD7F32"));
                }
                if(ServiceMedalTB.Text == "Silver")
                {
                    ServiceMedalTB.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF757575"));
                }
                if (ServiceMedalTB.Text == "Gold")
                {
                    ServiceMedalTB.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE3AB1C"));
                }
            }
                
        }
        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            EditGenderComboBox.SelectedItem = null;
            EditPostalCodeTextBox.Text = string.Empty;
            using (var dbContext = new MyDbContext())
            {
                var user = dbContext.Users.FirstOrDefault(u => u.Username == UserName);
                if (user != null)
                {
                    UsernameTB.Text = "Username: " + user.Username;
                    FirstnameTB.Text = "Firstname: " + user.FirstName;
                    PhoneTB.Text = "Phone: " + user.Phone;
                    LastnameTB.Text = "Lastname: " + user.LastName;
                    EmailTB.Text = "Email: " + user.Email;
                    GenderTB.Text = "Gender: " + user.Gender;
                    PostalCodeTB.Text = "Postal Code: " +  user.PostalCode;
                }
            }

            if (ProfileCanvas.Visibility == Visibility.Visible)
            {
                ProfileCanvas.Visibility = Visibility.Collapsed;
                EditCanvas.Visibility = Visibility.Collapsed;
                BronzeCanvas.Visibility = Visibility.Collapsed;
                SilverCanvas.Visibility = Visibility.Collapsed;
                GoldCanvas.Visibility = Visibility.Collapsed;
            }
            else
            {
                ProfileCanvas.Visibility = Visibility.Visible;
            }
            
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EditGenderComboBox.SelectedItem = null;
            EditPostalCodeTextBox.Text = string.Empty;
            if (EditCanvas.Visibility == Visibility.Visible)
            {
                EditCanvas.Visibility = Visibility.Collapsed;
                BronzeCanvas.Visibility = Visibility.Collapsed;
                SilverCanvas.Visibility = Visibility.Collapsed;
                GoldCanvas.Visibility = Visibility.Collapsed;
            }
            else
            {
                EditCanvas.Visibility = Visibility.Visible;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //GenderTB.Text = (string)EditGenderComboBox.SelectedItem;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            EditCanvas.Visibility = Visibility.Collapsed;
            EditGenderComboBox.SelectedItem = null;
            EditPostalCodeTextBox.Text = string.Empty;
            if (GoldCanvas.Visibility == Visibility.Visible)
            {
                GoldCanvas.Visibility = Visibility.Collapsed;
            }
            if (SilverCanvas.Visibility == Visibility.Visible)
            {
                GoldCanvas.Visibility= Visibility.Collapsed;
            }
            if (BronzeCanvas.Visibility == Visibility.Visible) { BronzeButton.Visibility = Visibility.Collapsed; }
        }

        private void EditApplyButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new MyDbContext())
            {
                var user = dbContext.Users.FirstOrDefault(u => u.Username == UserName);
                if (EditGenderComboBox.SelectedItem == null && string.IsNullOrEmpty(EditPostalCodeTextBox.Text))
                {
                    //nothing really
                }
                else if (EditGenderComboBox.SelectedItem != null && string.IsNullOrEmpty(EditPostalCodeTextBox.Text))
                {
                    string[] parts = ((ComboBoxItem)EditGenderComboBox.SelectedItem).ToString().Split(':');
                    string gender = parts[1];
                    user.Gender = gender;
                    dbContext.SaveChanges();
                    GenderTB.Text = "Gender: " + gender;
                }
                else if (EditGenderComboBox.SelectedItem == null && string.IsNullOrEmpty(EditPostalCodeTextBox.Text) == false)
                {
                    user.PostalCode = EditPostalCodeTextBox.Text;
                    PostalCodeTB.Text = "Postal Code: " + EditPostalCodeTextBox.Text;
                    dbContext.SaveChanges();
                }
                else if (EditGenderComboBox.SelectedItem != null && string.IsNullOrEmpty(EditPostalCodeTextBox.Text) == false)
                {
                    string[] parts = ((ComboBoxItem)EditGenderComboBox.SelectedItem).ToString().Split(':');
                    string gender = parts[1];
                    user.Gender = gender;
                    user.PostalCode = EditPostalCodeTextBox.Text;
                    dbContext.SaveChanges();
                    GenderTB.Text = "Gender: " + gender;
                    PostalCodeTB.Text = "Postal Code: " + EditPostalCodeTextBox.Text;
                }
            }

            
        }

        private void BronzeButton_Click(object sender, RoutedEventArgs e)
        {
            BronzeCanvas.Visibility = Visibility.Visible;
            SilverCanvas.Visibility = Visibility.Collapsed;
            GoldCanvas.Visibility = Visibility.Collapsed;
        }

        private void SilverButton_Click(object sender, RoutedEventArgs e)
        {
            SilverCanvas.Visibility= Visibility.Visible;
            BronzeCanvas.Visibility= Visibility.Collapsed;
            GoldCanvas.Visibility= Visibility.Collapsed;
        }

        private void GoldButton_Click(object sender, RoutedEventArgs e)
        {
            GoldCanvas.Visibility = Visibility.Visible;
            SilverCanvas.Visibility = Visibility.Collapsed;
            BronzeCanvas.Visibility = Visibility.Collapsed;
        }

        private void GoBronzeButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new MyDbContext())
            {
                var user = dbContext.Users.FirstOrDefault(u => u.Username == UserName);
                if (String.Equals(user.Service, "Bronze") == true)
                {

                    ExistingServiceCanvas.Visibility = Visibility.Visible;
                    ExistingOKButton.Visibility= Visibility.Visible;
                    ExistingYesButton.Visibility= Visibility.Collapsed;
                    ExistingNoButton.Visibility= Visibility.Collapsed;
                    BronzeOKButton.IsEnabled = false;
                    ExistingServiceTB.Text = "You have already subscribed to this service previously.";
                }
                else if (String.Equals(user.Service, "Normal") == true)
                {
                    user.Service = "Bronze";
                    ServiceMedalTB.Text = "Bronze";
                    ServiceMedalTB.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCD7F32"));
                    dbContext.SaveChanges();
                }
                else if (String.Equals(user.Service, "Silver") == true || String.Equals(user.Service, "Gold") == true)
                {
                    ExistingServiceCanvas.Visibility= Visibility.Visible;
                    ExistingOKButton.Visibility= Visibility.Visible;
                    ExistingYesButton.Visibility= Visibility.Collapsed;
                    ExistingNoButton.Visibility = Visibility.Collapsed;
                    BronzeOKButton.IsEnabled = false;
                    ExistingServiceTB.Text = "You've previously opted for a higher-tier service.";
                }
            }
            
        }

        private void GoSilverButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new MyDbContext())
            {
                var user = dbContext.Users.FirstOrDefault(u => u.Username == UserName);
                if (String.Equals(user.Service, "Silver") == true)
                {
                    ExistingServiceCanvas.Visibility = Visibility.Visible;
                    ExistingOKButton.Visibility = Visibility.Visible;
                    ExistingYesButton.Visibility = Visibility.Collapsed;
                    ExistingNoButton.Visibility = Visibility.Collapsed;
                    SilverOKButton.IsEnabled = false;
                    ExistingServiceTB.Text = "You have already subscribed to this service previously.";
                }
                else if (String.Equals(user.Service, "Normal") == true)
                {
                    user.Service = "Silver";
                    ServiceMedalTB.Text = "Silver";
                    ServiceMedalTB.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF757575"));
                    dbContext.SaveChanges();
                }
                else if (String.Equals(user.Service, "Gold") == true)
                {
                    ExistingServiceCanvas.Visibility = Visibility.Visible;
                    ExistingOKButton.Visibility = Visibility.Visible;
                    ExistingYesButton.Visibility = Visibility.Collapsed;
                    ExistingNoButton.Visibility = Visibility.Collapsed;
                    SilverOKButton.IsEnabled = false;
                    ExistingServiceTB.Text = "You've previously opted for a higher-tier service.";
                }
                else if (String.Equals(user.Service, "Bronze") == true)
                {
                    ExistingServiceCanvas.Visibility = Visibility.Visible;
                    ExistingOKButton.Visibility = Visibility.Collapsed;
                    ExistingYesButton.Visibility = Visibility.Visible;
                    ExistingNoButton.Visibility = Visibility.Visible;
                    SilverOKButton.IsEnabled = false;
                    ExistingServiceTB.Text = "If you proceed, your current service will be terminated. Are you sure you want to continue?";
                }
            }
            
        }

        private void GoGoldButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new MyDbContext())
            {
                var user = dbContext.Users.FirstOrDefault(u => u.Username == UserName);
                if (String.Equals(user.Service, "Gold") == true)
                {
                    ExistingServiceCanvas.Visibility = Visibility.Visible;
                    ExistingOKButton.Visibility = Visibility.Visible;
                    ExistingYesButton.Visibility = Visibility.Collapsed;
                    ExistingNoButton.Visibility = Visibility.Collapsed;
                    GoldOKButton.IsEnabled = false;
                    ExistingServiceTB.Text = "You have already subscribed to this service previously.";
                }
                else if (String.Equals(user.Service, "Normal") == true)
                {
                    user.Service = "Gold";
                    ServiceMedalTB.Text = "Gold";
                    ServiceMedalTB.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE3AB1C"));
                    dbContext.SaveChanges();
                }
                else if (String.Equals(user.Service, "Silver") == true || String.Equals(user.Service, "Bronze") == true)
                {
                    ExistingServiceCanvas.Visibility = Visibility.Visible;
                    ExistingOKButton.Visibility = Visibility.Collapsed;
                    ExistingYesButton.Visibility = Visibility.Visible;
                    ExistingNoButton.Visibility = Visibility.Visible;
                    GoldOKButton.IsEnabled = false;
                    ExistingServiceTB.Text = "If you proceed, your current service will be terminated. Are you sure you want to continue?";
                }
            }
            
        }

        private void ExistingOKButton_Click(object sender, RoutedEventArgs e)
        {
            BronzeOKButton.IsEnabled = true;
            SilverOKButton.IsEnabled = true;
            GoldOKButton.IsEnabled = true;
            BronzeButton.Visibility = Visibility.Visible;
            ExistingServiceCanvas.Visibility = Visibility.Collapsed;
        }
        private void ExistingYesButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new MyDbContext())
            {
                var user = dbContext.Users.FirstOrDefault(u => u.Username == UserName);
                if (BronzeCanvas.Visibility == Visibility.Visible)
                {
                    user.Service = "Bronze";
                    ServiceMedalTB.Text = "Bronze";
                    ServiceMedalTB.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCD7F32"));
                    dbContext.SaveChanges();
                }
                if (SilverCanvas.Visibility == Visibility.Visible)
                {
                    user.Service = "Silver";
                    ServiceMedalTB.Text = "Silver";
                    ServiceMedalTB.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF757575"));
                    dbContext.SaveChanges();
                }
                if (GoldCanvas.Visibility == Visibility.Visible)
                {
                    user.Service = "Gold";
                    ServiceMedalTB.Text = "Gold";
                    ServiceMedalTB.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE3AB1C"));
                    dbContext.SaveChanges();
                }
                ExistingServiceCanvas.Visibility = Visibility.Collapsed;
                SilverOKButton.IsEnabled = true;
                BronzeOKButton.IsEnabled = true;
                GoldOKButton.IsEnabled = true;
            }

        }
        private void ExistingNoButton_Click(object sender, RoutedEventArgs e)
        {
            ExistingServiceCanvas.Visibility = Visibility.Collapsed;
            SilverOKButton.IsEnabled = true;
            BronzeOKButton.IsEnabled = true;
            GoldOKButton.IsEnabled = true;
        }

        private void BronzeOKButton_Click(object sender, RoutedEventArgs e)
        {
            BronzeCanvas.Visibility = Visibility.Collapsed;
        }

        private void SilverOKButton_Click(object sender, RoutedEventArgs e)
        {
            SilverCanvas.Visibility = Visibility.Collapsed;
        }

        private void GoldOKButton_Click(object sender, RoutedEventArgs e)
        {
            GoldCanvas.Visibility = Visibility.Collapsed;
        }
        /****************************************************************************************************/
        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            RestaurantNamePlaceholder.Visibility = Visibility.Collapsed;
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameSearchTextBox.Text))
            {
                RestaurantNamePlaceholder.Visibility = Visibility.Visible;
            }
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }

        private void CityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void RestaurantTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void MinRatingSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MinRatingSlider != null && MinRatingTextBlock != null)
            {
                if (MinRatingSlider.Value == 0)
                {
                    MinRatingTextBlock.Text = $"Min Rating: 0";
                }
                else
                {
                    MinRatingTextBlock.Text = $"Min Rating: {MinRatingSlider.Value}";
                }
            }
        }

        private void LoadRestaurants()
        {
            using (var dbContext = new MyDbContext())
            {
                restaurants = dbContext.Restaurants.ToList();
                List<string> restaurantsnames = restaurants.Select(x => x.Name).ToList();
                RestaurantListBox.ItemsSource = restaurantsnames;
            }
        }
        private void LoadCities()
        {
            using (var dbContext = new MyDbContext())
            {
                var cities = dbContext.Restaurants.Select(r => r.City).Distinct().ToList();
                cities.Insert(0, "All"); //0 omin index insert mishe
                CityComboBox.ItemsSource = cities;
            }
        }
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> filteredRestaurants = new List<string>();

            var city = CityComboBox.SelectedItem as string;
            var minRating = MinRatingSlider.Value;
            var searchText = NameSearchTextBox.Text;

            string type = null;
            if (RestaurantTypeComboBox.SelectedItem != null)
            {
                string[] parts = RestaurantTypeComboBox.SelectedItem.ToString().Split(':');
                if (parts.Length > 1)
                {
                    type = parts[1].Trim();
                }
            }

            filteredRestaurants = restaurants.Where(r =>
                (string.IsNullOrEmpty(city) || city == "All" || r.City == city) &&
                r.Rating >= minRating &&
                (r.Name.StartsWith(searchText, StringComparison.OrdinalIgnoreCase) || r.Name.Contains(searchText)) &&
                (type == null || r.Type == type || r.Type == "Both")
            ).Select(r => r.Name).ToList();

            if (filteredRestaurants.Count == 0)
            {
                NameSearchTextBox.Text = string.Empty;  
                MinRatingSlider.Value = 1;
                RestaurantListBox.ItemsSource = null;
                MessageBox.Show("No restaurants available with this filter.", "Filter Result", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                RestaurantListBox.ItemsSource = filteredRestaurants;
            }
        }

        private void RestaurantListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RestaurantListBox.SelectedItem != null)
            {
                var selectedRestaurant = RestaurantListBox.SelectedItem.ToString();
                if (selectedRestaurant == "Jo")
                {
                    Restaurant restaurant;
                    using (var dbContext = new MyDbContext())
                    {
                        restaurant = dbContext.Restaurants.Where(r => r.Name == RestaurantListBox.SelectedItem.ToString()).FirstOrDefault();
                    }
                    AddressTB.Text = $"This restaurant is located in {restaurant.Address}.";
                    AddressCanvas.Visibility = Visibility.Visible;
                }
                
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            RestaurantListBox.ItemsSource = restaurants.Select(r => r.Name).ToList();
            CityComboBox.SelectedItem = null;
            RestaurantTypeComboBox.SelectedItem = null;
            MinRatingSlider.Value = 0;
            NameSearchTextBox.Text = string.Empty;
        }


        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void OKNavigationButton_Click(object sender, RoutedEventArgs e)
        {
            AddressCanvas.Visibility = Visibility.Collapsed;
            Jo JoPage = new Jo();
            JoPage.Username = UserName;
            ContentFrame.Navigate(JoPage);
        }

        private void CancelNavigationButton_Click(object sender, RoutedEventArgs e)
        {
            AddressCanvas.Visibility = Visibility.Collapsed;
        }
    }
}
