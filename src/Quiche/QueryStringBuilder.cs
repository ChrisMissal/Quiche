namespace Quiche
{
    using Settings;

    internal class QueryStringBuilder
    {
        private readonly ObjectBuilder _objectBuilder;

        internal QueryStringBuilder(BuilderSettings settings)
        {
            _objectBuilder = new ObjectBuilder(new PropertyBuilder(settings));
        }

        internal string Build(object value)
        {
            return "?" + _objectBuilder.GetObjectString(value);
        }
    }
}