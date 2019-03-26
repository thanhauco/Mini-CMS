using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MiniCMS.Infrastructure.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(Stream fileStream, string fileName, string contentType);
        Task<Stream> GetFileAsync(string storagePath);
        Task DeleteFileAsync(string storagePath);
        string GetFileHash(Stream fileStream);
    }

    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _basePath;

        public LocalFileStorageService(string basePath)
        {
            _basePath = basePath;
            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }

        public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string contentType)
        {
            var uniqueName = $"{Guid.NewGuid()}_{fileName}";
            var filePath = Path.Combine(_basePath, uniqueName);

            using (var file = File.Create(filePath))
            {
                await fileStream.CopyToAsync(file);
            }

            return uniqueName;
        }

        public Task<Stream> GetFileAsync(string storagePath)
        {
            var filePath = Path.Combine(_basePath, storagePath);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found", storagePath);
            }

            return Task.FromResult<Stream>(File.OpenRead(filePath));
        }

        public Task DeleteFileAsync(string storagePath)
        {
            var filePath = Path.Combine(_basePath, storagePath);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return Task.CompletedTask;
        }

        public string GetFileHash(Stream fileStream)
        {
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(fileStream);
                fileStream.Position = 0; // Reset stream position
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}
