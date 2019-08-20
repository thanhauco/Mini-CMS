using System;
using System.Collections.Generic;
using MiniCMS.Domain.Entities;
using MiniCMS.Domain.FieldTypes;
using Newtonsoft.Json.Linq;

namespace MiniCMS.Application.Services
{
    public interface IContentValidator
    {
        bool Validate(Schema schema, JObject data, out List<string> errors);
    }

    public class ContentValidator : IContentValidator
    {
        public bool Validate(Schema schema, JObject data, out List<string> errors)
        {
            errors = new List<string>();

            foreach (var field in schema.Fields)
            {
                var value = data[field.Name];
                
                if (value == null || value.Type == JTokenType.Null)
                {
                    if (field.IsRequired)
                    {
                        errors.Add($"Field '{field.Name}' is required.");
                    }
                    continue;
                }

                if (!field.Validate(value.ToString(), out string errorMessage))
                {
                    errors.Add(errorMessage);
                }
            }

            return errors.Count == 0;
        }
    }
}
