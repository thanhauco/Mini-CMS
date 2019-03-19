using System;
using MiniCMS.Domain.Common;

namespace MiniCMS.Domain.Entities
{
    /// <summary>
    /// Represents a media file/asset
    /// </summary>
    public class Asset : BaseEntity
    {
        public Guid AppId { get; private set; }
        
        public string FileName { get; private set; }
        
        public string MimeType { get; private set; }
        
        public long FileSize { get; private set; }
        
        public string FileHash { get; private set; }
        
        public string StoragePath { get; private set; }
        
        public string Slug { get; private set; }
        
        public string Tags { get; set; }
        
        public string Metadata { get; set; }

        private Asset() { }

        public Asset(Guid appId, string fileName, string mimeType, long fileSize, string storagePath)
        {
            AppId = appId;
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            MimeType = mimeType ?? "application/octet-stream";
            FileSize = fileSize;
            StoragePath = storagePath ?? throw new ArgumentNullException(nameof(storagePath));
            Slug = GenerateSlug(fileName);
        }

        private string GenerateSlug(string fileName)
        {
            var name = System.IO.Path.GetFileNameWithoutExtension(fileName);
            return name.ToLowerInvariant()
                .Replace(" ", "-")
                .Replace("_", "-");
        }

        public void SetFileHash(string hash)
        {
            FileHash = hash;
        }

        public void Rename(string newFileName)
        {
            if (string.IsNullOrWhiteSpace(newFileName))
                throw new ArgumentException("File name cannot be empty", nameof(newFileName));
            
            FileName = newFileName;
            Slug = GenerateSlug(newFileName);
            ModifiedAt = DateTime.UtcNow;
        }

        public bool IsImage()
        {
            return MimeType?.StartsWith("image/") == true;
        }

        public bool IsVideo()
        {
            return MimeType?.StartsWith("video/") == true;
        }

        public bool IsDocument()
        {
            return MimeType?.StartsWith("application/pdf") == true ||
                   MimeType?.StartsWith("application/msword") == true ||
                   MimeType?.Contains("document") == true;
        }
    }
}
