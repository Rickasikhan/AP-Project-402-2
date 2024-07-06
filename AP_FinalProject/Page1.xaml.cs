using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        
        //private Button Button2;
        public Page1()
        {
            InitializeComponent();
            this.Title = "Login Page";
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            WrongCredentialsTB.Visibility = Visibility.Collapsed;
            if (string.IsNullOrWhiteSpace(UsernameTexBox.Text) || string.IsNullOrWhiteSpace(PasswordPassBox.Password))
            {
                WrongCredentialsTB.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                WrongCredentialsTB.Visibility = Visibility.Collapsed;
            }
            using (var dbContext = new MyDbContext())
            {
                string username = UsernameTexBox.Text;
                string password = PasswordPassBox.Password;

                bool isValidUser = dbContext.Users.Any(u => u.Username == username && u.Password == password);

                if (isValidUser)
                {
                    //UserPanel userPanel = new UserPanel();
                    UserPanel userPanel = new UserPanel(username);
                    SubmitButton.Visibility = Visibility.Collapsed;
                    UsernameTexBox.Visibility = Visibility.Collapsed;
                    PasswordPassBox.Visibility = Visibility.Collapsed;
                    ContentFrame.Navigate(userPanel);
                }
                else
                {
                    WrongCredentialsTB.Visibility = Visibility.Visible;
                }
            }
        }
        private void UsernameTexBox_GotFocus(object sender, RoutedEventArgs e)
        {
            UsernamePlaceHolderTB.Visibility = Visibility.Collapsed;
        }

        private void UsernameTexBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UsernameTexBox.Text))
            {
                UsernamePlaceHolderTB.Visibility = Visibility.Visible;
            }
        }

        private void PasswordPassBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PsswordPlaceHolderTB.Visibility = Visibility.Collapsed;
        }

        private void PasswordPassBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PasswordPassBox.Password))
            {
                PsswordPlaceHolderTB.Visibility = Visibility.Visible;
            }
        }


        private void UsernameTexBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }

    }
}
