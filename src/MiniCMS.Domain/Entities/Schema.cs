using System;
using System.Collections.Generic;
using System.Linq;
using MiniCMS.Domain.Common;
using MiniCMS.Domain.FieldTypes;

namespace MiniCMS.Domain.Entities
{
    /// <summary>
    /// Represents a dynamic content type definition
    /// </summary>
    public class Schema : BaseEntity
    {
        public Guid AppId { get; private set; }
        
        public string Name { get; private set; }
        
        public string DisplayName { get; private set; }
        
        public string Description { get; set; }
        
        public bool IsPublished { get; private set; }
        
        public bool IsSingleton { get; private set; }
        
        public List<FieldDefinition> Fields { get; private set; } = new List<FieldDefinition>();

        private Schema() { }

        public Schema(Guid appId, string name, string displayName = null)
        {
            AppId = appId;
            SetName(name);
            DisplayName = displayName ?? name;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Schema name cannot be empty", nameof(name));
            
            if (name.Length > 50)
                throw new ArgumentException("Schema name cannot exceed 50 characters", nameof(name));
            
            Name = name.ToLowerInvariant().Replace(" ", "-");
        }

        public void AddField(FieldDefinition field)
        {
            if (Fields.Any(f => f.Name == field.Name))
                throw new InvalidOperationException($"Field '{field.Name}' already exists");
            
            field.Order = Fields.Count;
            Fields.Add(field);
            ModifiedAt = DateTime.UtcNow;
        }

        public void RemoveField(string fieldName)
        {
            var field = Fields.FirstOrDefault(f => f.Name == fieldName);
            if (field != null)
            {
                Fields.Remove(field);
                ReorderFields();
                ModifiedAt = DateTime.UtcNow;
            }
        }

        public void ReorderFields(List<string> fieldOrder)
        {
            for (int i = 0; i < fieldOrder.Count; i++)
            {
                var field = Fields.FirstOrDefault(f => f.Name == fieldOrder[i]);
                if (field != null)
                {
                    field.Order = i;
                }
            }
            ModifiedAt = DateTime.UtcNow;
        }

        private void ReorderFields()
        {
            for (int i = 0; i < Fields.Count; i++)
            {
                Fields[i].Order = i;
            }
        }

        public void Publish()
        {
            if (Fields.Count == 0)
                throw new InvalidOperationException("Cannot publish schema without fields");
            
            IsPublished = true;
            ModifiedAt = DateTime.UtcNow;
        }

        public void Unpublish()
        {
            IsPublished = false;
            ModifiedAt = DateTime.UtcNow;
        }

        public void SetAsSingleton(bool isSingleton)
        {
            IsSingleton = isSingleton;
            ModifiedAt = DateTime.UtcNow;
        }
    }
}
