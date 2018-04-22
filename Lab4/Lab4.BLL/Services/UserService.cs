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

        public string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null) {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8)) {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        public bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null) {
                return false;
            }
            if (password == null) {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0)) {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8)) {
                buffer4 = bytes.GetBytes(0x20);
            }
            return ByteArraysEqual(buffer3, buffer4);
        }

        private bool ByteArraysEqual(byte[] buffer3, byte[] buffer4)
        {
            if (buffer3.Length == 0)
                return false;
            if (buffer3.Length != buffer4.Length)
                return false;
            for (int i = 0; i < buffer3.Length; i++) {
                if (buffer3[i] != buffer4[i])
                    return false;
            }
            return true;
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
            var user = DbContext.Users
                .Where(u => u.Login == login)
                .FirstOrDefault();
            if (user == null)
                return null;
            if (VerifyHashedPassword(user.Password, password))
                return user;
            return null;
        }
    }
}
