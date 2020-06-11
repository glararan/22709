using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace repro
{
    public class ValidationContext<T> where T : class
    {
        public T Model { get; private set; }

        public EditContext EditContext { get; private set; }

        public Dictionary<string, List<string>> Errors { get; private set; } = new Dictionary<string, List<string>>();

        public ValidationContext(T model)
        {
            Model = model;

            EditContext = new EditContext(Model);
            EditContext.OnFieldChanged += EditContext_OnFieldChanged;
        }

        void EditContext_OnFieldChanged(object sender, FieldChangedEventArgs e)
        {
            PropertyInfo property = Model.GetType().GetProperty(e.FieldIdentifier.FieldName);

            if (property is null)
                return;

            IEnumerable<ValidationAttribute> attributes = property.GetCustomAttributes<ValidationAttribute>();

            List<string> errors = new List<string>();

            if (Errors.ContainsKey(e.FieldIdentifier.FieldName))
                Errors.Remove(e.FieldIdentifier.FieldName);

            foreach (ValidationAttribute attribute in attributes)
            {
                if (!attribute.IsValid(property.GetValue(Model)))
                    errors.Add(attribute.ErrorMessage);
            }

            if (errors.Count > 0)
                Errors[e.FieldIdentifier.FieldName] = errors;
        }
    }
}
