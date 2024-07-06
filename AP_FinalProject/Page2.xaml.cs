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
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows.Markup;
using Microsoft.Extensions.Configuration;
using System.Xml.Linq;

namespace AP_FinalProject
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        
        public Page2()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void PhoneTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        public bool AreAllTextBoxesFilled()
        {
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(LastNameTexBox.Text) ||
                string.IsNullOrWhiteSpace(PhoneTextBox.Text) ||
                string.IsNullOrWhiteSpace(E_mailTexBox.Text) ||
                string.IsNullOrWhiteSpace(UsernameTextBox.Text))
            {
                return false;
            }
            return true;
        }
        public static bool LengthNameValidError(string name)
        {
            if (name.Length < 3 || name.Length > 32)
            {
                return false;
            }

            return true;
        }
        public static bool PhoneNumberLength(string name)
        {
            if (name.Length != 11)
            {
                return false;
            }

            return true;
        }
        public static bool IsValidName(string name) //LengthNameValidError
        { 

            Regex regex = new Regex("^[a-zA-Z]+$");
            if (!regex.IsMatch(name))
            {
                return false;
            }

            return true;
        }
        public static bool IsValidEmail(string email)
        {
            string[] parts = email.Split(new char[] { '@', '.' });
            try
            {
                if (parts[0].Length >= 3 && parts[1].Length >= 3 && parts[2].Length >= 2 && parts[2].Length <= 3)
                {
                    if (IsValidEmailRegex(email) == true)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false ;
            }
            return false;
        }
        public static bool IsValidEmailRegex(string email)
        {
            Regex regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            return regex.IsMatch(email);
        }
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (!Regex.IsMatch(phoneNumber, @"^\d{10,11}$"))
            {
                return false;
            }
            string[] validPrefixes = { "0912", "0933", "0919", "0914", "09" }; 
            if (!validPrefixes.Any(prefix => phoneNumber.StartsWith(prefix)))
            {
                return false;
            }

            return true;
        }
        private void SubmitSignUpButton_Click(object sender, RoutedEventArgs e)
        {
            InvalidPhoneNumberTB.Visibility = Visibility.Collapsed;
            InvalidEmailTB.Visibility = Visibility.Collapsed;
            if (IsValidName(FirstNameTextBox.Text) == false)
            {
                IsValidNameErrorTB.Visibility = Visibility.Visible;
            }
            else
            {
                IsValidNameErrorTB.Visibility = Visibility.Collapsed;
            }
            if (LengthNameValidError(FirstNameTextBox.Text) == false)
            {
                LengthErrorNameTB.Visibility = Visibility.Visible;
            }
            else
            {
                LengthErrorNameTB.Visibility = Visibility.Collapsed;
            }
            /****************************/
            if (IsValidName(LastNameTexBox.Text) == false)
            {
                LastnameIsValidNameErrorTB.Visibility = Visibility.Visible;
            }
            else
            {
                LastnameIsValidNameErrorTB.Visibility = Visibility.Collapsed;
            }
            if (LengthNameValidError(LastNameTexBox.Text) == false)
            {
                LastnameLengthErrorNameTB.Visibility = Visibility.Visible;
            }
            else
            {
                LastnameLengthErrorNameTB.Visibility = Visibility.Collapsed;
            }
            /*************************/
            if (IsValidName(UsernameTextBox.Text) == false)
            {
                UsernameErrorTB.Visibility = Visibility.Visible;
            }
            else
            {
                UsernameErrorTB.Visibility = Visibility.Collapsed;
            }
            if (LengthNameValidError(UsernameTextBox.Text) == false)
            {
                UsernameLengthErrorTB.Visibility = Visibility.Visible;
            }
            else
            {
                UsernameLengthErrorTB.Visibility = Visibility.Collapsed;
            }
            /*************************/
            if (!AreAllTextBoxesFilled())
            {
                EmptyError.Visibility = Visibility.Visible;
            }
            else
            {
                EmptyError.Visibility = Visibility.Collapsed;
            }
            if (PhoneNumberLength(PhoneTextBox.Text) == false)
            {
                InvalidPhoneNumberTB.Visibility = Visibility.Visible;
            }
            else
            {
                InvalidPhoneNumberTB.Visibility = Visibility.Collapsed;
            }
            /*int parser;
            try
            {
                parser = int.Parse(PhoneTextBox.Text);
                InvalidPhoneNumberTB.Visibility = Visibility.Collapsed;
            }
            catch
            {
                InvalidPhoneNumberTB.Visibility = Visibility.Visible;
            }*/
            /***********************/
            if (IsValidPhoneNumber(PhoneTextBox.Text) == false)
            {
                InvalidPhoneNumberTB.Visibility = Visibility.Visible;
            }
            else
            {
                InvalidPhoneNumberTB.Visibility= Visibility.Collapsed;
            }
            /**********************/
            if (IsValidEmail(E_mailTexBox.Text) == false)
            {
                InvalidEmailTB.Visibility = Visibility.Visible;
            }
            else
            {
                InvalidEmailTB.Visibility= Visibility.Collapsed;
            }
            /***********************/
            if (EmptyError.Visibility == Visibility.Visible || LengthErrorNameTB.Visibility == Visibility.Visible || 
                InvalidPhoneNumberTB.Visibility == Visibility.Visible || IsValidNameErrorTB.Visibility == Visibility.Visible || 
                InvalidEmailTB.Visibility == Visibility.Visible || UsernameLengthErrorTB.Visibility == Visibility.Visible ||
                UsernameErrorTB.Visibility == Visibility.Visible)
            {
                ErrorTB.Visibility = Visibility.Visible;
                return; //ending the func because of existing errors
            }
            else
            {
                ErrorTB.Visibility = Visibility.Collapsed; //no handling if all errors are handled.
            }
            using (var dbContext = new MyDbContext())
            {
                var firstName = FirstNameTextBox.Text;
                var lastName = LastNameTexBox.Text;
                var phone = PhoneTextBox.Text;
                var email = E_mailTexBox.Text;
                var username = UsernameTextBox.Text;

                if ( dbContext.Users.Any(u => u.Phone == phone))
                {
                    PhoneNumberErrorTaken.Visibility = Visibility.Visible;
                    return;
                }
                else
                {
                    PhoneNumberErrorTaken.Visibility = Visibility.Collapsed;
                }
                if (dbContext.Users.Any(u => u.Username == username))
                {
                    UsernameErrorTaken.Visibility = Visibility.Visible;
                    return;
                }
                else
                {
                    UsernameErrorTaken.Visibility = Visibility.Collapsed;
                }
                if (UsernameError.Visibility == Visibility.Collapsed && PhoneError.Visibility == Visibility.Collapsed)
                {
                    Page3 VerificationPage = new Page3();
                    VerificationPage.Firstname = FirstNameTextBox.Text;
                    VerificationPage.Lastname = LastNameTexBox.Text;
                    VerificationPage.Username = UsernameTextBox.Text;
                    VerificationPage.Email = E_mailTexBox.Text;
                    VerificationPage.PhoneNumber = PhoneTextBox.Text;

                    Random random = new Random(DateTime.Now.Millisecond);
                    string senderEmail = "kasrakhalaj44@gmail.com";
                    string senderPassword = "murb dztj glho owmb";
                    string pass = $"{random.Next(100001, 999999).ToString()}";
                    string recipientEmail = $"{email}";
                    string subject = "Verfication code";
                    string body = $"Verfication code for KA restaurants manager \n            {pass}";

                    SendEmail(senderEmail, senderPassword, recipientEmail, subject, body);
                    
                    VerificationPage.VerficationPassword = pass;
                    VerificationPage.SenderEmail = senderEmail;
                    VerificationPage.SenderPassword = senderPassword;
                    VerificationPage.RecipentEmail = recipientEmail;
                    VerificationPage.Subject = subject;
                    VerificationPage.Body = body;
                    ContentFrame.Navigate(VerificationPage);
                }
            }
        }
        private void SendEmail(string senderEmail, string senderPassword, string recipientEmail, string subject, string body)
        {
            using (var smtpClient = new SmtpClient("smtp.gmail.com"))
            {
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                smtpClient.EnableSsl = true;
                //smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                try
                {
                    using (var mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(senderEmail);
                        mailMessage.To.Add(new MailAddress(recipientEmail));
                        mailMessage.Subject = subject;
                        mailMessage.Body = body;
                        try
                        {
                            smtpClient.Send(mailMessage);
                            ErrorDebuggerTB.Visibility = Visibility.Collapsed;
                        }
                        catch (Exception ex)
                        {
                            ErrorDebuggerTB.Visibility = Visibility.Visible;
                            ErrorDebuggerTB.Text = $"Error sending email: {ex.Message}";
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorDebuggerTB.Visibility = Visibility.Visible;
                    ErrorDebuggerTB.Text = $"Error initializing email client: {ex.Message}";
                }
            }
        }

        /*private void SendMail(string senderEmail, string senderPassword, string recipientEmail, string subject, string body)
        {

        }*/

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
        /*private void HideButtonsPage2()
{
   PhoneTextBox.Visibility = Visibility.Collapsed;
   SignUpButton.Visibility = Visibility.Collapsed;
   FiningExperienceTB.Visibility = Visibility.Collapsed;
   // Add more buttons here as needed
}*/

    }
}
