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
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        private List<Restaurant> restaurants;
        private List<Complaint> complaintsList;

        public AdminPage()
        {
            InitializeComponent();
            using (var dbContext = new MyDbContext())
            {
                restaurants = dbContext.Restaurants.ToList();
            }
            LoadData();
            
        }

        private void LoadData()
        {
            
            LoadRestaurants();
            LoadCities();
        }

        private void LoadRestaurants()
        {
            using (var dbContext = new MyDbContext())
            {
                restaurants = dbContext.Restaurants.ToList();
                List<string> restaurantsnames = restaurants.Select(x => x.Name).ToList();
                RestaurantListBox2.ItemsSource = restaurantsnames;
            }
        }
        private void LoadCities()
        {
            using (var dbContext = new MyDbContext())
            {
                var cities = dbContext.Restaurants.Select(r => r.City).Distinct().ToList();
                cities.Insert(0, "All");
                CityComboBox.ItemsSource = cities;
            }
        }
        private void LoadComplaints()
        {
            using (var dbContext = new MyDbContext())
            {
                complaintsList = dbContext.Complaints.ToList();
                if (complaintsList.Count == 0)
                {
                    MessageBox.Show("No complaints found.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    ComplaintsListBox.ItemsSource = complaintsList;
                }
            }
        }






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
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e) { }
        private void CityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        private void RestaurantTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
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
                RestaurantListBox2.ItemsSource = null;
                MessageBox.Show("No restaurants available with this filter.", "Filter Result", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                RestaurantListBox2.ItemsSource = filteredRestaurants;
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            RestaurantListBox2.ItemsSource = restaurants.Select(r => r.Name).ToList();
            CityComboBox.SelectedItem = null;
            RestaurantTypeComboBox.SelectedItem = null;
            MinRatingSlider.Value = 0;
            NameSearchTextBox.Text = string.Empty;
        }

        private void ApplyAddRestaurantButton_Click(object sender, RoutedEventArgs e)
        {
            string restaurantName = "temp";
            string city = "temp";
            string address = "temp";
            string type = "temp";

            try
            {
                restaurantName = AddRestaurantNameTexBox.Text.Trim();
                city = AddCityRestaurantTexBox.Text.Trim();
                address = AddressAddRestaurantTexBox.Text.Trim();

                if (AddTypeRestaurantComboBox.SelectedItem != null)
                {
                    type = AddTypeRestaurantComboBox.SelectedItem.ToString().Split(':')[1];
                }
                else
                {
                    MessageBox.Show("Please select a restaurant type.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(restaurantName) || string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(type))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int max = 0;

            using (var dbContext = new MyDbContext())
            {
                var restaurant = dbContext.Restaurants.OrderByDescending(r => r.RestaurantId).FirstOrDefault();
                if (restaurant != null)
                {
                    max = restaurant.RestaurantId;
                }
            }

            Restaurant newRestaurant = new Restaurant
            {
                Name = restaurantName,
                City = city,
                Address = address,
                Type = type,
                Rating = 0.0,
                RestaurantId = max + 1
            };


            using (var dbContext = new MyDbContext())
            {
                dbContext.Restaurants.Add(newRestaurant);
                dbContext.SaveChanges();
            }

            LoadRestaurants();
            LoadCities();
            AddRestaurantCanvas.Visibility = Visibility.Collapsed;
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e) { }
        private void AddRestaurantNameTexBox_TextChanged(object sender, TextChangedEventArgs e) { }
        private void AddCityRestaurantTexBox_TextChanged(object sender, TextChangedEventArgs e) { }


        private void AddRestaurantButton_Click(object sender, RoutedEventArgs e)
        {
            AddRestaurantCanvas.Visibility = Visibility.Visible;
        }

        private void CloseAddRestaurant_Click(object sender, RoutedEventArgs e)
        {
            AddRestaurantCanvas.Visibility=Visibility.Collapsed;
        }

        private void RemoveResButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new MyDbContext())
            {
                int restaurantId = int.Parse(IdRemvoveTextBox.Text.ToString().Trim());
                Restaurant restaurant = dbContext.Restaurants.Find(restaurantId);
                if (restaurant != null)
                {
                    dbContext.Restaurants.Remove(restaurant);
                    dbContext.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Restaurant not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            LoadRestaurants();
            LoadCities();

        }

        private void CloseRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveResCanvas.Visibility = Visibility.Collapsed;
        }

        private void RemoveRestaurantButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveResCanvas.Visibility=Visibility.Visible;
        }

        private void UpdateRestaurantButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateCanvas.Visibility = Visibility.Visible;
        }

        private void UpdateCloseButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateCanvas.Visibility = Visibility.Collapsed;
        }

        private void UpdateApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UpdateResIdTexBox.Text))
            {
                MessageBox.Show("Please fill the Res Id field!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int resid;
            if (!int.TryParse(UpdateResIdTexBox.Text.Trim(), out resid))
            {
                MessageBox.Show("Invalid Restaurant ID input", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var dbContext = new MyDbContext())
            {
                var res = dbContext.Restaurants.Find(resid);
                if (res == null)
                {
                    MessageBox.Show("Restaurant not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!string.IsNullOrEmpty(UpdateNameTextBox.Text))
                {
                    res.Name = UpdateNameTextBox.Text;
                }

                if (!string.IsNullOrEmpty(UpdateAddressTexBox.Text))
                {
                    res.Address = UpdateAddressTexBox.Text;
                }

                if (!string.IsNullOrEmpty(UpdateCityTexBox.Text))
                {
                    res.City = UpdateCityTexBox.Text;
                }

                if (UpdateTypeComboBox.SelectedItem != null)
                {
                    res.Type = UpdateTypeComboBox.SelectedItem.ToString().Split(':')[1];
                }

                dbContext.SaveChanges();
            }

            LoadRestaurants();
            LoadCities();
            UpdateCanvas.Visibility = Visibility.Collapsed;
        }

        private void RestaurantListBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RestaurantListBox2.SelectedItem != null)
            {
                string selectedRestaurantName = RestaurantListBox2.SelectedItem.ToString();
                MessageBox.Show($"Selected Restaurant: {selectedRestaurantName}");
            }
        }
        private void CheckComplaintCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                Complaint selectedComplaint = checkBox.DataContext as Complaint;
                if (selectedComplaint != null)
                {
                    selectedComplaint.IsCheckedOut = true;
                    UpdateComplaint(selectedComplaint);
                }
            }
        }

        private void CheckComplaintCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                Complaint selectedComplaint = checkBox.DataContext as Complaint;
                if (selectedComplaint != null)
                {
                    selectedComplaint.IsCheckedOut = false;
                    UpdateComplaint(selectedComplaint);
                }
            }
        }

        private void UpdateComplaint(Complaint complaint)
        {
            using (var dbContext = new MyDbContext())
            {
                var existingComplaint = dbContext.Complaints.Find(complaint.ComplaintId);
                if (existingComplaint != null)
                {
                    existingComplaint.IsCheckedOut = complaint.IsCheckedOut;
                    dbContext.SaveChanges();
                }
            }
        }
        private void SearchComplaintsButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchComplaintsTextBox.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                LoadComplaints();
            }
            else
            {
                using (var dbContext = new MyDbContext())
                {
                    var complaints = dbContext.Complaints
                                        .Where(c => c.User.Username.ToLower().Contains(searchText)
                                                 || c.Restaurant.Name.ToLower().Contains(searchText)
                                                 || c.Subject.ToLower().Contains(searchText))
                                        .ToList();

                    // Explicitly load related entities (User and Restaurant)
                    foreach (var complaint in complaints)
                    {
                        dbContext.Entry(complaint).Reference(c => c.User).Load();
                        dbContext.Entry(complaint).Reference(c => c.Restaurant).Load();
                    }

                    if (complaints.Count == 0)
                    {
                        MessageBox.Show("No matching complaints found.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    ComplaintsListBox.ItemsSource = complaints;
                }
            }
        }



        private void SearchComplaintsTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchPlaceholder.Visibility = Visibility.Collapsed;
        }
        private void SearchComplaintsTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchPlaceholder.Visibility = Visibility.Visible;
        }

        private void ComplaintsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComplaintsListBox.SelectedItem != null)
            {
                Complaint selectedComplaint = ComplaintsListBox.SelectedItem as Complaint;
                if (selectedComplaint != null)
                {
                    MessageBox.Show($"Selected Complaint:\n\nSubject: {selectedComplaint.Subject}\nUser: {selectedComplaint.User.Username}\nRestaurant: {selectedComplaint.Restaurant.Name}\nDescription: {selectedComplaint.Description}",
                                    "Selected Complaint", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void AddResAdminButton_Click(object sender, RoutedEventArgs e)
        {
            AddResAdminCanvas.Visibility = Visibility.Visible;
        }

        private void AddAdminCloseButton_Click(object sender, RoutedEventArgs e)
        {
            AddResAdminCanvas.Visibility = Visibility.Collapsed;
        }

        private void AddAdminAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddAdminResIdTexBox.Text == string.Empty)
            {
                MessageBox.Show("Please select a restaurant Id.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (AddResAdminUsernameTextBox.Text == string.Empty || AddResAdminPassTextBox.Text == string.Empty || AddResAdminUsernameTextBox.Text == null || AddResAdminPassTextBox.Text == null)
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int validresid = 0;
            try
            {
                validresid = int.Parse(AddAdminResIdTexBox.Text.Trim());
            }
            catch
            {
                MessageBox.Show("Please inpput a valid restaurant Id.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Restaurant res;
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    res = dbContext.Restaurants.FirstOrDefault(r => r.RestaurantId == validresid);
                }
                if (res == null)
                {
                    MessageBox.Show("Please inpput a valid restaurant Id.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Please inpput a valid restaurant Id.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (validresid == 1)
            {
                using (var dbContext = new MyDbContext())
                {
                    var joadmin = dbContext.JoAdmins.FirstOrDefault(k => k.Username == AddResAdminUsernameTextBox.Text.Trim());
                    if (joadmin == null)
                    {
                        JoAdmin newJoAdmin = new JoAdmin
                        {
                            Username = AddResAdminUsernameTextBox.Text,
                            Password = AddResAdminPassTextBox.Text,
                        };
                        dbContext.JoAdmins.Add(newJoAdmin);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        MessageBox.Show("Username is taken.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    
                    dbContext.SaveChanges();
                }
            }
            AddResAdminCanvas.Visibility = Visibility.Collapsed;

        }

        private void ChangeResAdminButton_Click(object sender, RoutedEventArgs e)
        {
            ChnageAdminResPassCanvas.Visibility = Visibility.Visible;
        }

        private void ApplyNewPassButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(NewAdminPassTextBox.Text) || String.IsNullOrEmpty(NewPassAdminRepeatTextBox.Text) || String.IsNullOrEmpty(NewPassAdminIdTextBox.Text) || String.IsNullOrEmpty(NewPassResIDTextBox.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (NewAdminPassTextBox.Text != NewPassAdminRepeatTextBox.Text)
            {
                MessageBox.Show("passwords does not match.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            using (var dbContext = new MyDbContext())
            {
                int id = int.Parse(NewPassResIDTextBox.Text.Trim());
                var targetres = dbContext.Restaurants.FirstOrDefault(r => r.RestaurantId == id);
                if (targetres == null)
                {
                    MessageBox.Show($"No restaurant with the given id.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                int adminid = int.Parse(NewPassAdminIdTextBox.Text.Trim());
                if (targetres.RestaurantId == 1)
                {
                    var admin = dbContext.JoAdmins.FirstOrDefault(j => j.JoAdminId == adminid);
                    if (admin == null)
                    {
                        MessageBox.Show($"No restaurant with the given id.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    admin.Password = NewPassAdminRepeatTextBox.Text.Trim();
                    dbContext.SaveChanges();
                }
            }
            ChnageAdminResPassCanvas.Visibility= Visibility.Collapsed;
        }

        private void CloseNewPassButton_Click(object sender, RoutedEventArgs e)
        {
            ChnageAdminResPassCanvas.Visibility=Visibility.Collapsed;
        }
    }
}
