using System;
using MiniCMS.Domain.Common;

namespace MiniCMS.Domain.Entities
{
    public class Webhook : BaseEntity
    {
        public Guid AppId { get; private set; }
        
        public string Name { get; private set; }
        
        public string Url { get; private set; }
        
        public string Secret { get; private set; }
        
        public bool IsEnabled { get; private set; } = true;
        
        public WebhookEvent Events { get; private set; }

        private Webhook() { }

        public Webhook(Guid appId, string name, string url, WebhookEvent events, string secret = null)
        {
            AppId = appId;
            Name = name;
            Url = url;
            Events = events;
            Secret = secret ?? Guid.NewGuid().ToString("N");
        }

        public void Update(string name, string url, WebhookEvent events, bool isEnabled)
        {
            Name = name;
            Url = url;
            Events = events;
            IsEnabled = isEnabled;
            ModifiedAt = DateTime.UtcNow;
        }
    }

    [Flags]
    public enum WebhookEvent
    {
        ContentCreated = 1,
        ContentUpdated = 2,
        ContentDeleted = 4,
        ContentPublished = 8,
        SchemaCreated = 16,
        SchemaUpdated = 32,
        All = -1
    }
}
