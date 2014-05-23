﻿namespace Quiche
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Settings;

    internal class QueryStringBuilder
    {
        private readonly FieldBuilder _fieldBuilder = new FieldBuilder();

        private readonly FieldCasing _fieldCasing;
        private readonly Func<string, string> _customFieldConverter;

        private readonly IDictionary<FieldCasing, Func<string, string>> _fieldConverters = new Dictionary<FieldCasing, Func<string, string>>
        {
            { FieldCasing.Default, s => s },
            { FieldCasing.CamelCase, ConvertToCamelCase },
            { FieldCasing.Custom, null },
        };

        public QueryStringBuilder(BuilderSettings settings)
        {
            _fieldCasing = settings.FieldCasing;
            _customFieldConverter = settings.CustomFieldConverter ?? _fieldConverters[FieldCasing.Default];
        }

        public string Build(object value)
        {
            return "?" + GetObjectString(value);
        }

        private string GetObjectString(object value, params string[] parentFields)
        {
            var props = value.GetType().GetProperties();
            var values = props.Select(property =>
            {
                if (!property.PropertyType.IsArray)
                    return BuildQueryStringFromObject(value, property, parentFields);

                var propertyValue = property.GetValue(value, null);
                var objects = ((IEnumerable)propertyValue).Cast<object>().ToArray();

                if (objects.Select(x => x.GetType()).Distinct().Count() == 1)
                    return objects.Aggregate("", (s, i) => s + GetPropertyQueryString(i, property.Name, parentFields));

                return objects.Select((i, index) => new Tuple<int, object>(index, i)).Aggregate("", (s, pair) =>
                {
                    var i = pair.Item2;
                    var propertyName = property.Name;
                    if (i is ValueType)
                    {
                        propertyName += "[]";
                        return s + GetPropertyQueryString(i, propertyName, parentFields);
                    }
                    else
                    {
                        parentFields = new[] { propertyName, Convert.ToString(pair.Item1) }.Concat(parentFields).ToArray();
                        return s + GetObjectString(i, parentFields).TrimEnd('&');
                    }
                });
            });
            var joined = string.Join("", values);
            return (parentFields == null || parentFields.Length == 0) ? joined.TrimEnd('&') : joined;
        }

        private string BuildQueryStringFromObject(object value, PropertyInfo property, params string[] parentFields)
        {
            var fieldName = property.Name;
            var propertyValue = property.GetValue(value, null);

            if (propertyValue is ValueType || propertyValue is string)
                return GetPropertyQueryString(propertyValue, fieldName, parentFields);

            var propString = GetObjectString(propertyValue, parentFields.Concat(new[] { fieldName }).ToArray());

            return propString;
        }

        private string GetPropertyQueryString(object propertyValue, string fieldName, params string[] parentFields)
        {
            return (parentFields != null && parentFields.Length > 0)
                        ? GetPropertyValueQuerySTring(propertyValue, fieldName, parentFields)
                        : GetSimpleValueQueryString(propertyValue, fieldName);
        }

        private string GetPropertyValueQuerySTring(object value, string field, params string[] parentFields)
        {
            var fieldConverter = _fieldConverters[_fieldCasing] ?? _customFieldConverter;
            var greatestAncestorField = parentFields.First();
            var descendantFields = parentFields.Concat(new[] { field })
                .Skip(1)
                .Aggregate("", (s, f) => string.Format("{0}[{1}]", s, fieldConverter(f)));

            return _fieldBuilder.Build(value,greatestAncestorField + descendantFields, fieldConverter);
        }

        private string GetSimpleValueQueryString(object value, string field)
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