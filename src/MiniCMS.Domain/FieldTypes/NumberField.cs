using System;

namespace MiniCMS.Domain.FieldTypes
{
    /// <summary>
    /// Number field for integer and decimal values
    /// </summary>
    public class NumberField : FieldDefinition
    {
        public override FieldType Type => FieldType.Number;
        
        public double? MinValue { get; set; }
        
        public double? MaxValue { get; set; }
        
        public bool AllowDecimals { get; set; } = true;

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

            if (!double.TryParse(value.ToString(), out double numValue))
            {
                errorMessage = $"Field '{Name}' must be a valid number";
                return false;
            }

            if (!AllowDecimals && numValue != Math.Floor(numValue))
            {
                errorMessage = $"Field '{Name}' must be an integer";
                return false;
            }

            if (MinValue.HasValue && numValue < MinValue.Value)
            {
                errorMessage = $"Field '{Name}' must be at least {MinValue}";
                return false;
            }

            if (MaxValue.HasValue && numValue > MaxValue.Value)
            {
                errorMessage = $"Field '{Name}' must not exceed {MaxValue}";
                return false;
            }

            return true;
        }
    }
}
