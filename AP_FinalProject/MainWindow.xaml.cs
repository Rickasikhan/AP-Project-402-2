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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Title = "Restaurant Managaer";
            LoginButton = this.FindName("LoginButton") as Button;
            SignUpButton = this.FindName("SignUpButton") as Button;
            FiningExperienceTB = this.FindName("FiningExperienceTB") as TextBlock;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the login page
            HideButtonsMainWindow();
            ContentFrame.Navigate(new Page1());
        }

        private void HideButtonsMainWindow()
        {
            LoginButton.Visibility = Visibility.Collapsed;
            SignUpButton.Visibility = Visibility.Collapsed;
            FiningExperienceTB.Visibility = Visibility.Collapsed;
            // Add more buttons here as needed
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {

            // Navigate to the sign-up page
            HideButtonsMainWindow();
            ContentFrame.Navigate(new Page2());
        }
    }

}
