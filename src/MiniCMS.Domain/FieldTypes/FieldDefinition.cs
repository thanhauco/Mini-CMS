using System;

namespace MiniCMS.Domain.FieldTypes
{
    /// <summary>
    /// Base class for all field definitions
    /// </summary>
    public abstract class FieldDefinition
    {
        public string Name { get; set; }
        
        public string DisplayName { get; set; }
        
        public string Description { get; set; }
        
        public bool IsRequired { get; set; }
        
        public bool IsLocalizable { get; set; }
        
        public int Order { get; set; }
        
        public abstract FieldType Type { get; }

        public abstract bool Validate(object value, out string errorMessage);
    }

    public enum FieldType
    {
        String,
        Number,
        Boolean,
        DateTime,
        Assets,
        References,
        Array,
        Json
    }
}
