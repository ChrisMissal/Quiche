namespace Quiche
{
    using System.Linq;

    internal class ArrayBuilder
    {
        private readonly PropertyBuilder _propertyBuilder;

        public ArrayBuilder(PropertyBuilder propertyBuilder)
        {
            _propertyBuilder = propertyBuilder;
        }

        public string Build(Parameter parameter)
        {
            return parameter.Objects.Aggregate("", (s, i) => s + _propertyBuilder.Build(i, parameter.Property.Name, parameter.Parents));
        }
    }
}