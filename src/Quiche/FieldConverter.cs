namespace Quiche
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Settings;

    internal class FieldConverter
    {
        private readonly Func<string, string> _converter;

        private readonly IDictionary<FieldCasing, Func<string, string>> _fieldConverters = new Dictionary<FieldCasing, Func<string, string>>
        {
            { FieldCasing.Default, s => s },
            { FieldCasing.CamelCase, ConvertToCamelCase },
        };

        internal FieldConverter(BuilderSettings settings)
        {
            var converter = settings.CustomFieldConverter ?? _fieldConverters[settings.FieldCasing];

            _converter = settings.FieldArray == FieldArray.UseArraySyntax
                ? (s => converter(s) + "[]")
                : converter;
        }

        private static string ConvertToCamelCase(string field)
        {
            var firstLetter = field.Substring(0, 1).ToLowerInvariant();
            return field.ToCharArray().Skip(1).Aggregate(firstLetter, (s, c) => s + c);
        }

        internal string Convert(string field)
        {
            return _converter(field);
        }
    }
}