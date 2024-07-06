using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for PasswordConfirmer.xaml
    /// </summary>
    public partial class PasswordConfirmer : Page
    {
        public string Username;
        public string Password;
        public string Email;
        public string PhoneNumber;
        public string Firstname;
        public string Lastname;
        public PasswordConfirmer()
        {
            InitializeComponent();
        }

        private void ProceedButton_Click(object sender, RoutedEventArgs e)
        {
            if (FirstPassBox.Password != SecondPassBox.Password)
            {
                PasswordMatchTB.Visibility = Visibility.Visible;
                return;
            }
            else
            {

                PasswordMatchTB.Visibility = Visibility.Collapsed;
            }
            if (IsValidPassword(FirstPassBox.Password) == false)
            {
                PassRegexTB.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                PassRegexTB.Visibility = Visibility.Collapsed;
            }
            PassRepeatTB.Visibility = Visibility.Collapsed;
            var user = new User(Username, Email, PhoneNumber, Firstname, Lastname); 
            user.Password = FirstPassBox.Password;
            using (var db = new MyDbContext())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }

            UserPanel userPanel = new UserPanel(Username);
            ContentFrame.Navigate(userPanel);
        }

        public static bool IsValidPassword(string password)
        {
            const int MIN_LENGTH = 8;
            const int MAX_LENGTH = 32;

            if (password.Length < MIN_LENGTH || password.Length > MAX_LENGTH)
            {
                return false;
            }

            var hasUpperCase = new Regex(@"[A-Z]+");
            var hasLowerCase = new Regex(@"[a-z]+");
            var hasDigit = new Regex(@"[0-9]+");

            if (!hasUpperCase.IsMatch(password) || !hasLowerCase.IsMatch(password) || !hasDigit.IsMatch(password))
            {
                return false;
            }

            return true;
        }
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
