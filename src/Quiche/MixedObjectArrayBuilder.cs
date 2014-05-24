namespace Quiche
{
    using System;
    using System.Linq;
    using Settings;

    internal class MixedObjectArrayBuilder
    {
        private readonly PropertyBuilder _propertyBuilder;
        private Func<object, string[], string> _builder;

        public MixedObjectArrayBuilder(BuilderSettings settings, PropertyBuilder propertyBuilder)
        {
            _propertyBuilder = propertyBuilder;
        }

        internal string Build(Parameter parameter)
        {
            var objectArray = parameter.Objects;
            return objectArray.Select((i, index) => new Tuple<int, object>(index, i))
                .Aggregate("", (s, pair) =>
                {
                    var i = pair.Item2;
                    if (i is ValueType)
                    {
                        return s + _propertyBuilder.Build(i, "", new[] { parameter.Property.Name });
                    }
                    parameter.InsertParents(Convert.ToString(pair.Item1), parameter.Property.Name);
                    return s + _builder(i, parameter.Parents).TrimEnd('&');
                });
        }

        public void SetBuilder(Func<object, string[], string> builder)
        {
            _builder = builder;
        }
    }
}