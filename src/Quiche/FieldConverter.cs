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
            { FieldCasing.Custom, null },
        };

        internal FieldConverter(BuilderSettings settings)
        {
            _converter = settings.CustomFieldConverter ?? _fieldConverters[settings.FieldCasing];
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