namespace Quiche
{
    using System.Linq;
    using Settings;

    internal class PropertyBuilder
    {
        private readonly FieldBuilder _fieldBuilder = new FieldBuilder();
        private readonly FieldConverter _converter;

        internal PropertyBuilder(BuilderSettings settings)
        {
            _converter = new FieldConverter(settings);
        }

        internal Field Build(object propertyValue, string fieldName, params string[] parentFields)
        {
            return (parentFields != null && parentFields.Length > 0)
                        ? GetPropertyValueQueryString(propertyValue, fieldName, parentFields)
                        : GetSimpleValueQueryString(propertyValue, fieldName);
        }

        private Field GetPropertyValueQueryString(object value, string field, params string[] parentFields)
        {
            var greatestAncestorField = parentFields.First();
            var descendantFields = parentFields.Concat(new[] { field })
                .Skip(1)
                .Aggregate("", (s, f) => string.Format("{0}[{1}]", s, _converter.Convert(f)));

            return _fieldBuilder.Build(value, greatestAncestorField + descendantFields, _converter);
        }

        private Field GetSimpleValueQueryString(object value, string field)
        {
            return _fieldBuilder.Build(value, field, _converter);
        }
    }
}