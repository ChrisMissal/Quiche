namespace Quiche
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
            _arrayBuilder.SetBuilder(GetObjectString);
        }

        internal Field Build(Parameter parameter)
        {
            var property = parameter.Property;
            var parentFields = parameter.Parents;

            var fieldName = property.Name;
            var propertyValue = property.GetValue(parameter.Value, null);

            if (propertyValue is ValueType || propertyValue is string)
                return _propertyBuilder.Build(propertyValue, fieldName, parentFields);

            if (propertyValue == null)
                return _nullBuilder.Build(fieldName, parentFields);

            return new MultiField(GetObjectString(propertyValue, parentFields.Concat(new[] { fieldName }).ToArray()));
        }

        internal IEnumerable<Field> GetObjectString(object value, params string[] parentFields)
        {
            var props = value.GetType().GetProperties();

            var parameters = props.Select(property => new Parameter(value, property).AddParent(parentFields));

            foreach (var parameter in parameters)
            {
                if (!parameter.IsArray)
                    yield return Build(parameter);

                else if (parameter.IsSingleTypeArray)
                    foreach (var field in _arrayBuilder.Build(parameter))
                        yield return field;

                else if (parameter.AreArrayObjectsValueTypes)
                    foreach (var field in _mixedObjectArrayBuilder.Build(parameter))
                        yield return field;

                else throw new QuicheBuilderException(parameter);
            }
        }
    }
}