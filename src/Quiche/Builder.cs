namespace Quiche
{
    using System;
    using Settings;

    public class Builder
    {
        private readonly QueryStringBuilder _qsBuilder;

        public Builder(Action<BuilderSettings> settingsConfig)
        {
            var settings = new BuilderSettings();
            settingsConfig(settings);
            _qsBuilder = new QueryStringBuilder(settings);
        }

        public Builder(BuilderSettings settings)
        {
            _qsBuilder = new QueryStringBuilder(settings);
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
