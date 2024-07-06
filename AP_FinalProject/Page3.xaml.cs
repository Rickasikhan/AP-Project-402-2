using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
    /// Interaction logic for Page3.xaml
    /// </summary>
    public partial class Page3 : Page
    {
        public string VerficationPassword { get; set; }
        public string SenderEmail;
        public string SenderPassword;
        public string RecipentEmail;
        public string Subject;
        public string Body;

        public string Username;
        public string Password;
        public string Email;
        public string PhoneNumber;
        public string Firstname;
        public string Lastname;


        public Page3()
        {

            InitializeComponent();
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
                            //ErrorDebuggerTB.Visibility = Visibility.Collapsed;
                        }
                        catch (Exception ex)
                        {
                            //ErrorDebuggerTB.Visibility = Visibility.Visible;
                            //ErrorDebuggerTB.Text = $"Error sending email: {ex.Message}";
                        }
                    }
                }
                catch (Exception ex)
                {
                    //ErrorDebuggerTB.Visibility = Visibility.Visible;
                    //ErrorDebuggerTB.Text = $"Error initializing email client: {ex.Message}";
                }
            }
        }
        private void ResendTBHyperLink_Click(object sender, RoutedEventArgs e)
        {
            // Perform an action here
            Random random = new Random(DateTime.Now.Millisecond);
            string pass = $"{random.Next(100001, 999999).ToString()}";
            Body = $"Verfication code for KA restaurants manager \n            {pass}";
            SendEmail(SenderEmail, SenderPassword, RecipentEmail, Subject, Body);
        }
        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            if (VerficationPassword == VerificationPB.Password)
            {
                ContinueButton.Visibility = Visibility.Collapsed;
                WrongPassTB.Visibility = Visibility.Collapsed;


                PasswordConfirmer passwordConfirmer = new PasswordConfirmer();
                passwordConfirmer.Password = VerificationPB.Password;
                passwordConfirmer.Firstname = Firstname;
                passwordConfirmer.Lastname = Lastname;
                passwordConfirmer.Email = Email;
                passwordConfirmer.Username = Username;
                passwordConfirmer.PhoneNumber = PhoneNumber;
                ContentFrame.Navigate(passwordConfirmer);
            }
            else
            {
                WrongPassTB.Visibility = Visibility.Visible;
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }

}
