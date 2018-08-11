using Lab4.BLL.Models;
using Lab4.BLL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Lab4.BLL.Services
{
    public class UserService
    {
        protected string ConnectionString;

        public UserService(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public User SignUp(User user)
        {
            if (user == null)
                throw new ArgumentNullException("User can't be null");
            string emptyFieldName = null;
            if (string.IsNullOrEmpty(user.Login))
                emptyFieldName = "Login";
            if (string.IsNullOrEmpty(user.Password))
                emptyFieldName = "Password";
            if (string.IsNullOrEmpty(user.FirstName))
                emptyFieldName = "FirstName";
            if (string.IsNullOrEmpty(user.Email))
                emptyFieldName = "Email";
            if (!string.IsNullOrEmpty(emptyFieldName))
                throw new ArgumentException($"Field {emptyFieldName} must be filled");

            user.Password = CryptoUtils.HashPassword(user.Password);
            using (var dbConext = new SocialNetDbContext(ConnectionString)) {
                user = dbConext.Users.Add(user).Entity;
                dbConext.SaveChanges();

                return dbConext.Entry(user).Entity;
            }
        }

        public User SignIn(string login, string password)
        {
            var hashedPassword = CryptoUtils.HashPassword(password);
            User user = null;
            using (var dbContext = new SocialNetDbContext(ConnectionString)) {
                user = dbContext.Users
                    .FirstOrDefault(u => u.Login == login);
            }
            if (user == null)
                return null;
            return CryptoUtils.VerifyHashedPassword(user.Password, password) ? user : null;
        }
    }
}
