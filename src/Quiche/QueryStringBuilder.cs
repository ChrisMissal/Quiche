namespace Quiche
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Reflection;
    using Settings;

    internal class QueryStringBuilder
    {
        private readonly PropertyBuilder _propertyBuilder;

        internal QueryStringBuilder(BuilderSettings settings)
        {
            _propertyBuilder = new PropertyBuilder(settings);
        }

        internal string Build(object value)
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
                    return objects.Aggregate("", (s, i) => s + _propertyBuilder.Build(i, property.Name, parentFields));

                return objects.Select((i, index) => new Tuple<int, object>(index, i)).Aggregate("", (s, pair) =>
                {
                    var i = pair.Item2;
                    var propertyName = property.Name;
                    if (i is ValueType)
                    {
                        propertyName += "[]";
                        return s + _propertyBuilder.Build(i, propertyName, parentFields);
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
                return _propertyBuilder.Build(propertyValue, fieldName, parentFields).ToString();

            var propString = GetObjectString(propertyValue, parentFields.Concat(new[] { fieldName }).ToArray());

            return propString;
        }
    }
}