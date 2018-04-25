using Lab4.BLL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lab4.BLL.Services
{
    class UserPhotoService
    {
        protected string ConnectionString;
        protected string FileStoragePath;

        public UserPhotoService(string connectionString, string fileStoragePath)
        {
            ConnectionString = connectionString;
        }

        private DirectoryInfo CreateUserPhotoDirectoryIfNotExists(int userId)
        {
            string directoryPath = FileStoragePath + "\\" + userId.ToString();
            if (Directory.Exists(directoryPath)) {
                return new DirectoryInfo(directoryPath);
            }
            else {
                return Directory.CreateDirectory(directoryPath);
            }
        }

        private string GetNotExistedFileName(DirectoryInfo directory, string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            var name = fileName.Substring(0, fileName.Length - fileExtension.Length);
            var similarFileNameRegexp = new Regex($@"^{name}(\(([0-9])+\))?\{fileExtension}", RegexOptions.IgnoreCase);
            var similarFileName = directory.GetFiles()
                .Where((f) => similarFileNameRegexp.IsMatch(f.Name))
                .OrderBy((f) => f.Name).LastOrDefault().Name;

            return $"{name}({similarFileNameRegexp.Match(similarFileName).Groups[2]}){fileExtension}";
        }

        private string SaveFile(int userId, string fileName, Stream inputStream)
        {
            var directory = CreateUserPhotoDirectoryIfNotExists(userId);

            fileName = GetNotExistedFileName(directory, fileName);

            string fullFilePath = directory.FullName + "\\" + fileName;
            using (var output = new FileStream(fullFilePath, FileMode.OpenOrCreate, FileAccess.Write)) {
                inputStream.CopyTo(output);
            }

            return fileName;
        }

        public UserPhoto UploadPhoto(int userId, string fileName, Stream inputStream)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName can't be empty");
            if (inputStream == null)
                throw new ArgumentNullException("Stream can't be null");
            if (inputStream.Length == 0)
                throw new ArgumentNullException("Stream can't be empty");

            fileName = SaveFile(userId, fileName, inputStream);

            UserPhoto userPhoto;
            using (var db = new SocialNetDbContext(ConnectionString)) {
                userPhoto = db.UserPhotos.Add(new UserPhoto() {
                    UserId = userId,
                    Name = fileName
                }).Entity;
                db.SaveChanges();
            }
            return userPhoto;
        }
    }
}
