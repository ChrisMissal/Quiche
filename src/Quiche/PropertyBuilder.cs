namespace Quiche
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Settings;

    internal class PropertyBuilder
    {
        private readonly IDictionary<FieldCasing, Func<string, string>> _fieldConverters = new Dictionary<FieldCasing, Func<string, string>>
        {
            { FieldCasing.Default, s => s },
            { FieldCasing.CamelCase, ConvertToCamelCase },
            { FieldCasing.Custom, null },
        };

        private readonly FieldBuilder _fieldBuilder = new FieldBuilder();

        private readonly FieldCasing _fieldCasing;
        private readonly Func<string, string> _customFieldConverter;

        internal PropertyBuilder(BuilderSettings settings)
        {
            _fieldCasing = settings.FieldCasing;
            _customFieldConverter = settings.CustomFieldConverter ?? _fieldConverters[FieldCasing.Default];
        }

        internal Field Build(object propertyValue, string fieldName, params string[] parentFields)
        {
            return (parentFields != null && parentFields.Length > 0)
                        ? GetPropertyValueQuerySTring(propertyValue, fieldName, parentFields)
                        : GetSimpleValueQueryString(propertyValue, fieldName);
        }

        private Field GetPropertyValueQuerySTring(object value, string field, params string[] parentFields)
        {
            var fieldConverter = _fieldConverters[_fieldCasing] ?? _customFieldConverter;
            var greatestAncestorField = parentFields.First();
            var descendantFields = parentFields.Concat(new[] { field })
                .Skip(1)
                .Aggregate("", (s, f) => string.Format("{0}[{1}]", s, fieldConverter(f)));

            return _fieldBuilder.Build(value,greatestAncestorField + descendantFields, fieldConverter);
        }

        private Field GetSimpleValueQueryString(object value, string field)
        {
            return _fieldBuilder.Build(value, field, _fieldConverters[_fieldCasing] ?? _customFieldConverter);
        }

        private static string ConvertToCamelCase(string field)
        {
            var firstLetter = field.Substring(0, 1).ToLowerInvariant();
            return field.ToCharArray().Skip(1).Aggregate(firstLetter, (s, c) => s + c);
        }
    }
}