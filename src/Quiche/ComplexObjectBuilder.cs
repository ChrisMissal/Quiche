namespace Quiche
{
    using System;
    using System.Linq;
    using System.Reflection;

    internal class ComplexObjectBuilder
    {
        private readonly ArrayBuilder _arrayBuilder;
        private readonly MixedObjectArrayBuilder _mixedObjectArrayBuilder;
        private readonly PropertyBuilder _propertyBuilder;
        private readonly NullBuilder _nullBuilder;

        internal ComplexObjectBuilder(PropertyBuilder propertyBuilder, NullBuilder nullBuilder, MixedObjectArrayBuilder mixedObjectArrayBuilder, ArrayBuilder arrayBuilder)
        {
            _propertyBuilder = propertyBuilder;
            _nullBuilder = nullBuilder;
            _mixedObjectArrayBuilder = mixedObjectArrayBuilder;
            _arrayBuilder = arrayBuilder;

            _mixedObjectArrayBuilder.SetBuilder(GetObjectString);
        }

        internal string Build(Parameter parameter)
        {
            var property = parameter.Property;
            var parentFields = parameter.Parents;

            var fieldName = property.Name;
            var propertyValue = property.GetValue(parameter.Value, null);

            if (propertyValue is ValueType || propertyValue is string)
                return _propertyBuilder.Build(propertyValue, fieldName, parentFields).ToString();

            if (propertyValue == null)
                return _nullBuilder.Build(fieldName, parentFields).ToString();

            var propString = GetObjectString(propertyValue, parentFields.Concat(new[] { fieldName }).ToArray());

            return propString;
        }

        internal string GetObjectString(object value, params string[] parentFields)
        {
            var props = value.GetType().GetProperties();
            
            var values = props.Select(property => new Parameter(value, property).AddParent(parentFields))
                .Select(parameter =>
                {
                    if (!parameter.IsArray)
                        return Build(parameter);

                    if (parameter.IsSingleTypeArray)
                        return _arrayBuilder.Build(parameter);

                    if (parameter.AreArrayObjectsValueTypes)
                        return _mixedObjectArrayBuilder.Build(parameter);

                    throw new QuicheBuilderException(parameter);
                });
            
            var joined = string.Join("", values);
            return (parentFields == null || parentFields.Length == 0) ? joined.TrimEnd('&') : joined;
        }
    }
}