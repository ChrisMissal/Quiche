namespace Quiche
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using Settings;

    internal class QueryStringBuilder
    {
        private readonly FieldCasing _fieldCasing;

        private readonly IDictionary<FieldCasing, Func<string, string>> _fieldConverters = new Dictionary<FieldCasing, Func<string, string>>
        {
            { FieldCasing.Default, s => s },
            { FieldCasing.CamelCase, ConvertToCamelCase },
        };

        public QueryStringBuilder(FieldCasing fieldCasing)
        {
            _fieldCasing = fieldCasing;
        }

        public string Build(object value)
        {
            var props = value.GetType().GetProperties();
            var values = props.Select(property => BuildQueryStringFromObject(value, property));
            return "?" + string.Join("", values).TrimEnd('&');
        }

        private string BuildQueryStringFromObject(object value, PropertyInfo property)
        {
            if (!property.PropertyType.IsArray)
                return GetQueryString(property.GetValue(value, null), property.Name);

            var objects = ((IEnumerable)property.GetValue(value, null)).Cast<object>();
            return objects.Aggregate("", (s, i) => s + GetQueryString(i, property.Name));
        }

        private string GetQueryString(object value, string field)
        {
            var url = value != null ? value.ToString() : "";
            var fieldValue = _fieldConverters[_fieldCasing](field);

            return string.Format("{0}={1}&", fieldValue, HttpUtility.UrlEncode(url));
        }

        private static string ConvertToCamelCase(string field)
        {
            var firstLetter = field.Substring(0, 1).ToLowerInvariant();
            return field.ToCharArray().Skip(1).Aggregate(firstLetter, (s, c) => s + c);
        }
    }
}