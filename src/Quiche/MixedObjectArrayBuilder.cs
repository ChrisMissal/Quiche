namespace Quiche
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Settings;

    internal class MixedObjectArrayBuilder
    {
        private readonly PropertyBuilder _propertyBuilder;
        private Func<object, string[], IEnumerable<Field>> _builder;

        public MixedObjectArrayBuilder(BuilderSettings settings, PropertyBuilder propertyBuilder)
        {
            _propertyBuilder = propertyBuilder;
        }

        internal IEnumerable<Field> Build(Parameter parameter)
        {
            var arrayLength = parameter.Objects.Length;
            for (var index = 0; index < arrayLength; index++)
            {
                var obj = parameter.Objects[index];
                if (obj is ValueType)
                {
                    yield return _propertyBuilder.Build(obj, "", new[] { parameter.Property.Name });
                }
                else
                {
                    parameter.InsertParents(Convert.ToString(index), parameter.Property.Name);
                    foreach (var field in _builder(obj, parameter.Parents))
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