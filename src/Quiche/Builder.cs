namespace Quiche
{
    using Settings;

    public class Builder
    {
        private readonly QueryStringBuilder _qsBuilder;

        public Builder(BuilderSettings settings)
        {
            _qsBuilder = new QueryStringBuilder(settings.FieldCasing);
        }

        public Builder() : this(BuilderSettings.DefaultSettings)
        {
        }

        public string ToQueryString(object obj)
        {
            return _qsBuilder.Build(obj);
        }
    }
}
