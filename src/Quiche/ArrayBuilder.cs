namespace Quiche
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class ArrayBuilder
    {
        private readonly PropertyBuilder _propertyBuilder;
        private Func<object, string[], IEnumerable<Field>> _builder;

        public ArrayBuilder(PropertyBuilder propertyBuilder)
        {
            _propertyBuilder = propertyBuilder;
        }

        public IEnumerable<Field> Build(Parameter parameter)
        {
            var arrayLength = parameter.Objects.Length;
            for (var index = 0; index < arrayLength; index++)
            {
                var obj = parameter.Objects[index];
                if (obj is ValueType || obj is string)
                    yield return _propertyBuilder.Build(obj, parameter.Property.Name, parameter.Parents);
                else
                {
                    foreach (var field in _builder(obj, new[] { parameter.Property.Name, Convert.ToString(index) }))
                        yield return field;
                }
            }
        }

        public void SetBuilder(Func<object, string[], IEnumerable<Field>> builder)
        {
            _builder = builder;
        }
    }
}