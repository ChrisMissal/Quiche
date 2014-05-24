namespace Quiche
{
    using Settings;

    internal class QueryStringBuilder
    {
        private readonly ObjectBuilder _objectBuilder;

        internal QueryStringBuilder(BuilderSettings settings)
        {
            var propertyBuilder = new PropertyBuilder(settings);
            var nullBuilder = new NullBuilder(settings);

            _objectBuilder = new ObjectBuilder(propertyBuilder, nullBuilder);
        }

        internal string Build(object value)
        {
            return "?" + _objectBuilder.GetObjectString(value);
        }
    }
}