using Lab4.BLL.Models;
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

        private string HashPassword(string password)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(password);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++) {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
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

            user.Password = HashPassword(user.Password);

            user = DbContext.Users.Add(user).Entity;
            DbContext.SaveChanges();

            return DbContext.Entry(user).Entity;
        }

        public User SignIn(string login, string password)
        {
            var hashedPassword = HashPassword(password);
            return DbContext.Users
                .Where(u => u.Login == login && u.Password == hashedPassword)
                .FirstOrDefault();
        }
    }
}
