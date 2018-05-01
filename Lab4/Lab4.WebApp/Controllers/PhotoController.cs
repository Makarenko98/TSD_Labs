using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Lab4.BLL.Services;
using Microsoft.AspNetCore.Authorization;

namespace Lab4.WebApp.Controllers
{
    [Produces("application/json")]
    [Route("api/Photo")]
    [Authorize]
    public class PhotoController : Controller
    {
        public FileResult GetUserProfilePhoto(int userId)
        {
            var photoService = new UserPhotoService(
                ConfigurationUtils.DefaultConnectionString,
                ConfigurationUtils.FileStoragePath);
            return new FileStreamResult(photoService.GetUserProfilePhoto(userId), "image");
        }

        public void UploadUserProfilePhoto(int userId)
        {

        }
    }
}