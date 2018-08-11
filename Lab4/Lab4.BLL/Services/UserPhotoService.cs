using Lab4.BLL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lab4.BLL.Services
{
    public class UserPhotoService
    {
        protected readonly string ConnectionString;
        protected readonly string FileStoragePath;

        public UserPhotoService(string connectionString, string fileStoragePath)
        {
            ConnectionString = connectionString;
            FileStoragePath = fileStoragePath;
        }

        private string GetUserPhotoDirectoryPath(int userId)
        {
            return FileStoragePath + "\\" + userId.ToString();
        }

        private DirectoryInfo CreateUserPhotoDirectoryIfNotExists(int userId)
        {
            string directoryPath = GetUserPhotoDirectoryPath(userId);
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
                                      .OrderBy((f) => f.Name).LastOrDefault()
                                      ?.Name ?? "";

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
                throw new ArgumentNullException(nameof(fileName) + " can't be empty");
            if (inputStream == null || inputStream.Length == 0)
                throw new ArgumentNullException(nameof(inputStream) + " can't be null or empty");

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

        public Stream GetPhoto(UserPhoto userPhoto)
        {
            var path = GetUserPhotoDirectoryPath(userPhoto.UserId);
            if (File.Exists(path)) {
                return new FileStream(path, FileMode.Open, FileAccess.Read);
            }

            return null;
        }

        public Stream GetUserProfilePhoto(int userId)
        {
            using (var db = new SocialNetDbContext(ConnectionString)) {
                return GetPhoto(db.Users.Find(userId).ProfilePhoto);
            }
        }
    }
}