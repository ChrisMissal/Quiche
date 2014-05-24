namespace Quiche
{
    using Settings;

    internal class QueryStringBuilder
    {
        private readonly ComplexObjectBuilder _complexObjectBuilder;

        internal QueryStringBuilder(BuilderSettings settings)
        {
            var propertyBuilder = new PropertyBuilder(settings);
            var nullBuilder = new NullBuilder(settings);
            var mixedArrayObjectBuilder = new MixedObjectArrayBuilder(settings, propertyBuilder);
            var arrayBuilder = new ArrayBuilder(propertyBuilder);

            _complexObjectBuilder = new ComplexObjectBuilder(propertyBuilder, nullBuilder, mixedArrayObjectBuilder, arrayBuilder);
        }

        internal string Build(object value)
        {
            return "?" + _complexObjectBuilder.GetObjectString(value);
        }
    }
}