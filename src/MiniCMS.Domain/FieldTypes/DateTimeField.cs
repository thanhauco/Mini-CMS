using System;

namespace MiniCMS.Domain.FieldTypes
{
    /// <summary>
    /// DateTime field for date and time values
    /// </summary>
    public class DateTimeField : FieldDefinition
    {
        public override FieldType Type => FieldType.DateTime;
        
        public DateTime? MinValue { get; set; }
        
        public DateTime? MaxValue { get; set; }
        
        public DateTimeMode Mode { get; set; } = DateTimeMode.DateTime;

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

            DateTime dateValue;
            if (value is DateTime dt)
            {
                dateValue = dt;
            }
            else if (!DateTime.TryParse(value.ToString(), out dateValue))
            {
                errorMessage = $"Field '{Name}' must be a valid date/time";
                return false;
            }

            if (MinValue.HasValue && dateValue < MinValue.Value)
            {
                errorMessage = $"Field '{Name}' must be after {MinValue.Value:yyyy-MM-dd}";
                return false;
            }

            if (MaxValue.HasValue && dateValue > MaxValue.Value)
            {
                errorMessage = $"Field '{Name}' must be before {MaxValue.Value:yyyy-MM-dd}";
                return false;
            }

            return true;
        }
    }

    public enum DateTimeMode
    {
        Date,
        Time,
        DateTime
    }
}
