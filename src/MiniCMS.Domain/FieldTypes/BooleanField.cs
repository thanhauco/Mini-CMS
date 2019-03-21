using System;

namespace MiniCMS.Domain.FieldTypes
{
    /// <summary>
    /// Boolean field for true/false values
    /// </summary>
    public class BooleanField : FieldDefinition
    {
        public override FieldType Type => FieldType.Boolean;
        
        public bool DefaultValue { get; set; } = false;

        public override bool Validate(object value, out string errorMessage)
        {
            errorMessage = null;
            
            if (value == null)
            {
                if (IsRequired)
                {
                    errorMessage = $"Field '{Name}' is required";
                    return false;
                }
                return true;
            }

            if (value is bool)
            {
                return true;
            }

            var strValue = value.ToString().ToLowerInvariant();
            if (strValue == "true" || strValue == "false" || strValue == "1" || strValue == "0")
            {
                return true;
            }

            errorMessage = $"Field '{Name}' must be a boolean value";
            return false;
        }

        public bool ParseValue(object value)
        {
            if (value == null) return DefaultValue;
            if (value is bool b) return b;
            
            var strValue = value.ToString().ToLowerInvariant();
            return strValue == "true" || strValue == "1";
        }
    }
}
