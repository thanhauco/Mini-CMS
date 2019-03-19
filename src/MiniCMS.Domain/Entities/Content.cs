using System;
using System.Collections.Generic;
using MiniCMS.Domain.Common;

namespace MiniCMS.Domain.Entities
{
    /// <summary>
    /// Represents a content item based on a schema
    /// </summary>
    public class Content : BaseEntity
    {
        public Guid AppId { get; private set; }
        
        public Guid SchemaId { get; private set; }
        
        public ContentStatus Status { get; private set; } = ContentStatus.Draft;
        
        public int Version { get; private set; } = 1;
        
        /// <summary>
        /// JSON data containing field values
        /// </summary>
        public string Data { get; private set; }
        
        public DateTime? PublishedAt { get; private set; }
        
        public string PublishedBy { get; private set; }

        private Content() { }

        public Content(Guid appId, Guid schemaId, string data)
        {
            AppId = appId;
            SchemaId = schemaId;
            SetData(data);
        }

        public void SetData(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                throw new ArgumentException("Content data cannot be empty", nameof(data));
            
            Data = data;
            Version++;
            ModifiedAt = DateTime.UtcNow;
        }

        public void Publish(string userId)
        {
            if (Status == ContentStatus.Published)
                throw new InvalidOperationException("Content is already published");
            
            Status = ContentStatus.Published;
            PublishedAt = DateTime.UtcNow;
            PublishedBy = userId;
            ModifiedAt = DateTime.UtcNow;
        }

        public void Unpublish()
        {
            if (Status != ContentStatus.Published)
                throw new InvalidOperationException("Content is not published");
            
            Status = ContentStatus.Draft;
            ModifiedAt = DateTime.UtcNow;
        }

        public void Archive()
        {
            Status = ContentStatus.Archived;
            ModifiedAt = DateTime.UtcNow;
        }

        public void Restore()
        {
            if (Status != ContentStatus.Archived)
                throw new InvalidOperationException("Content is not archived");
            
            Status = ContentStatus.Draft;
            ModifiedAt = DateTime.UtcNow;
        }
    }

    public enum ContentStatus
    {
        Draft,
        Published,
        Archived
    }
}
