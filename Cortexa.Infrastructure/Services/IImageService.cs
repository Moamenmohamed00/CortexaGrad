using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Cortexa.Application.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cortexa.Infrastructure.Services
{
    public interface IImageService
    {
        Task<(string Url, string PublicId)> UploadImageAsync(Stream stream, string fileName);
        Task<bool> DeleteImageAsync(string publicId);
    }

    public class ImageService : IImageService
    {
        private readonly Cloudinary _cloudinary;

        public ImageService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(account);
        }

        public async Task<(string Url, string PublicId)> UploadImageAsync(Stream stream, string fileName)
        {
            if (stream != null && stream.Length > 0)
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(fileName, stream),
                    Folder = "Medical/Imaging",
                    Transformation = new Transformation().Quality("auto"),
                    UseFilename = true,
                    UniqueFilename = true,
                    Overwrite = false
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                    throw new Exception(uploadResult.Error.Message);

                return (uploadResult.SecureUrl.ToString(), uploadResult.PublicId);
            }

            return (string.Empty, string.Empty);
        }

        public async Task<bool> DeleteImageAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result.Result == "ok";
        }
    }
}
