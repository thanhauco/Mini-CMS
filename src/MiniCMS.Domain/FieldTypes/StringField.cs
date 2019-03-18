using System;
using System.Text.RegularExpressions;

namespace MiniCMS.Domain.FieldTypes
{
    /// <summary>
    /// String field with text validation
    /// </summary>
    public class StringField : FieldDefinition
    {
        public override FieldType Type => FieldType.String;
        
        public int? MinLength { get; set; }
        
        public int? MaxLength { get; set; }
        
        public string Pattern { get; set; }
        
        public StringFieldEditor Editor { get; set; } = StringFieldEditor.Input;

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

            var strValue = value.ToString();

            if (MinLength.HasValue && strValue.Length < MinLength.Value)
            {
                errorMessage = $"Field '{Name}' must be at least {MinLength} characters";
                return false;
            }

            if (MaxLength.HasValue && strValue.Length > MaxLength.Value)
            {
                errorMessage = $"Field '{Name}' must not exceed {MaxLength} characters";
                return false;
            }

            if (!string.IsNullOrEmpty(Pattern))
            {
                if (!Regex.IsMatch(strValue, Pattern))
                {
                    errorMessage = $"Field '{Name}' does not match required pattern";
                    return false;
                }
            }

            return true;
        }
    }

    public enum StringFieldEditor
    {
        Input,
        TextArea,
        RichText,
        Markdown,
        Html
    }
}
