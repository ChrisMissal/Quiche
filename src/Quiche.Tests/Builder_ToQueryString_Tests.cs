namespace Quiche.Tests
{
    using Features;
    using Samples;
    using Settings;
    using Shouldly;

    public class Builder_ToQueryString_Tests
    {
        [Active("Turn flat object into a query string")]
        public void FlatObject_should_return_expected_string()
        {
            var builder = new Builder();
            var result = builder.ToQueryString(new FlatObject { Id = 12, Name = "Quiche" });

            result.ShouldBe("?Id=12&Name=Quiche");
        }

        [Active("Turn object with array into a query string")]
        public void ObjectWithArray_should_return_expected_string()
        {
            var builder = new Builder();
            var result = builder.ToQueryString(new ObjectWithArray { Items = new []{ 12, 34, 56 }});

            result.ShouldBe("?Items=12&Items=34&Items=56");
        }

        [Active("Turn object into query string with camel case fields")]
        public void ObjectWithArray_should_return_expected_string_with_camel_case_fields()
        {
            var settings = new BuilderSettings { FieldCasing = FieldCasing.CamelCase };
            var builder = new Builder(settings);
            var result = builder.ToQueryString(new MixedObject { ItemNames = new[] { "one", "two" }, TextMessage = "programming is fun" });

            result.ShouldBe("?itemNames=one&itemNames=two&textMessage=programming+is+fun");
        }
    }
}
