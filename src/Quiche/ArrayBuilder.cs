namespace Quiche
{
    using System.Collections.Generic;
    using System.Linq;

    internal class ArrayBuilder
    {
        private readonly PropertyBuilder _propertyBuilder;

        public ArrayBuilder(PropertyBuilder propertyBuilder)
        {
            _propertyBuilder = propertyBuilder;
        }

        public IEnumerable<Field> Build(Parameter parameter)
        {
            return parameter.Objects
                .Select(o => _propertyBuilder.Build(o, parameter.Property.Name, parameter.Parents));
        }
    }
}