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
        protected SocialNetDbContext DbContext;

        public UserService(SocialNetDbContext dbContext)
        {
            DbContext = dbContext;
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

            user = DbContext.Users.Add(user).Entity;
            DbContext.SaveChanges();

            return DbContext.Entry(user).Entity;
        }

        public User SignIn(string login, string password)
        {
            var hashedPassword = CryptoUtils.HashPassword(password);
            var user = DbContext.Users
                .Where(u => u.Login == login)
                .FirstOrDefault();
            if (user == null)
                return null;
            if (CryptoUtils.VerifyHashedPassword(user.Password, password))
                return user;
            return null;
        }
    }
}
