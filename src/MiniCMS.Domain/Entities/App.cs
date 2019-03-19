using System;
using System.Collections.Generic;
using MiniCMS.Domain.Common;

namespace MiniCMS.Domain.Entities
{
    /// <summary>
    /// Represents a multi-tenant application container
    /// </summary>
    public class App : BaseEntity
    {
        public string Name { get; private set; }
        
        public string DisplayName { get; private set; }
        
        public string Description { get; set; }
        
        public bool IsArchived { get; private set; }
        
        public List<AppContributor> Contributors { get; private set; } = new List<AppContributor>();
        
        public List<ApiKey> ApiKeys { get; private set; } = new List<ApiKey>();

        private App() { }

        public App(string name, string displayName)
        {
            SetName(name);
            DisplayName = displayName ?? name;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("App name cannot be empty", nameof(name));
            
            if (name.Length > 50)
                throw new ArgumentException("App name cannot exceed 50 characters", nameof(name));
            
            // Ensure name is URL-safe
            Name = name.ToLowerInvariant().Replace(" ", "-");
        }

        public void UpdateDisplayName(string displayName)
        {
            if (string.IsNullOrWhiteSpace(displayName))
                throw new ArgumentException("Display name cannot be empty", nameof(displayName));
            
            DisplayName = displayName;
            ModifiedAt = DateTime.UtcNow;
        }

        public void Archive()
        {
            IsArchived = true;
            ModifiedAt = DateTime.UtcNow;
        }

        public void Restore()
        {
            IsArchived = false;
            ModifiedAt = DateTime.UtcNow;
        }

        public void AddContributor(string userId, ContributorRole role)
        {
            if (Contributors.Exists(c => c.UserId == userId))
                throw new InvalidOperationException("User is already a contributor");
            
            Contributors.Add(new AppContributor(userId, role));
        }

        public void RemoveContributor(string userId)
        {
            var contributor = Contributors.Find(c => c.UserId == userId);
            if (contributor != null)
            {
                Contributors.Remove(contributor);
            }
        }

        public ApiKey GenerateApiKey(string name)
        {
            var apiKey = new ApiKey(Id, name);
            ApiKeys.Add(apiKey);
            return apiKey;
        }
    }

    public class AppContributor
    {
        public string UserId { get; private set; }
        public ContributorRole Role { get; private set; }
        public DateTime JoinedAt { get; private set; }

        private AppContributor() { }

        public AppContributor(string userId, ContributorRole role)
        {
            UserId = userId;
            Role = role;
            JoinedAt = DateTime.UtcNow;
        }
    }

    public enum ContributorRole
    {
        Owner,
        Developer,
        Editor,
        Viewer
    }

    public class ApiKey
    {
        public Guid Id { get; private set; }
        public Guid AppId { get; private set; }
        public string Name { get; private set; }
        public string Key { get; private set; }
        public string Secret { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? ExpiresAt { get; set; }
        public bool IsActive { get; private set; } = true;

        private ApiKey() { }

        public ApiKey(Guid appId, string name)
        {
            Id = Guid.NewGuid();
            AppId = appId;
            Name = name;
            Key = GenerateKey();
            Secret = GenerateSecret();
            CreatedAt = DateTime.UtcNow;
        }

        private string GenerateKey()
        {
            return $"mc_{Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("=", "").Replace("+", "").Replace("/", "")}";
        }

        private string GenerateSecret()
        {
            var bytes = new byte[32];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return Convert.ToBase64String(bytes).Replace("=", "").Replace("+", "").Replace("/", "");
        }

        public void Revoke()
        {
            IsActive = false;
        }
    }
}
