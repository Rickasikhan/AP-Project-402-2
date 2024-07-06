using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_FinalProject
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        private string _username;
        private string _password;
        private string _email;
        private string _phone;
        private string _firstname;
        private string _lastname;
        private string _gender;
        private string _postalCode;
        private string _service;

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }
        public string FirstName
        {
            get { return _firstname; }
            set { _firstname = value; }
        }
        public string LastName
        {
            get { return _lastname; }
            set { _lastname = value; }
        }
        public string Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }
        public string PostalCode
        {
            get { return _postalCode; }
            set { _postalCode = value; }
        }
        public string Service
        {
            get { return _service; }
            set { _service = value; }
        }
        public User(string username, string email, string phone, string firstname, string lastname)
        {
            _username = username;
            //_password = password; will get after the class has been created.
            _email = email;
            _phone = phone;
            _service = "Normal";
            _firstname = firstname;
            _lastname = lastname;
            _gender = "Not initialized";
            _postalCode = "Not initialized";
        }
        public User()
        {
            _gender = "Not initialized";
            _postalCode = "Not initialized";
        }
        /*public int GenerateUniqueUserId()
        {
            using (var db = new MyDbContext())
            {
                int userId = 0;
                bool isUnique = false;

                while (!isUnique)
                {
                    userId = new Random().Next(1, int.MaxValue);
                    isUnique = !db.Users.Any(u => u.UserId == userId);
                }

                return userId;
            }
        }*/

    }
}
