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

        [Active("Turn object into query string with custom field converter")]
        public void FlatObject_should_use_custom_converter_func()
        {
            var builder = new Builder(x =>
            {
                x.FieldCasing = FieldCasing.Custom;
                x.CustomFieldConverter = s => s.ToUpper();
            });
            var result = builder.ToQueryString(new { Test = "blah" });

            result.ShouldBe("?TEST=blah");
        }

        [Active("Turn object into query string with camel case fields")]
        public void MixedObject_should_return_expected_string_with_camel_case_fields()
        {
            var settings = new BuilderSettings { FieldCasing = FieldCasing.CamelCase };
            var builder = new Builder(settings);
            var result = builder.ToQueryString(new MixedObject { ItemNames = new[] { "one", "two" }, TextMessage = "programming is fun" });

            result.ShouldBe("?itemNames=one&itemNames=two&textMessage=programming+is+fun");
        }

        [Active("Turn object value into dictionary")]
        public void ObjectWithChildObject_should_return_expected_string()
        {
            var builder = new Builder(x =>
            {
                x.FieldCasing = FieldCasing.CamelCase;
            });
            var result = builder.ToQueryString(new ObjectWithChildObject { Child = new FlatObject { Id = 12, Name = "Fred Jr" }, Name = "Fred" });

            result.ShouldBe("?child[id]=12&child[name]=Fred+Jr&name=Fred");
        }

        [Active("Turn object with deep graph into deep nested key")]
        public void ObjectWithChildObject_with_many_descendants_should_return_expected_string()
        {
            var build = new Builder();
            var obj = new { Atmosphere = new { Crust = new { UpperMantle = new { TransitionZone = new { LowerMantle = new { OuterCore = new { InnerCore = "hot", } } } } } } };
            var result = build.ToQueryString(obj);

            result.ShouldBe("?Atmosphere[Crust][UpperMantle][TransitionZone][LowerMantle][OuterCore][InnerCore]=hot");
        }

        public void Simple_parent_child_object_should_return_expected_string()
        {
            var settings = new BuilderSettings { FieldCasing = FieldCasing.CamelCase };
            var builder = new Builder(settings);
            var result = builder.ToQueryString(new { a = new { b = 1 } });

            result.ShouldBe("?a[b]=1");
        }

        public void Complex_object_array_should_return_expected_string()
        {
            var builder = new Builder();
            var obj = new { a = new { b = 1, c = 2 }, d = new object[] { 3, 4, new { e = 5 } } };
            var result = builder.ToQueryString(obj);

            result.ShouldBe("?a[b]=1&a[c]=2&d[]=3&d[]=4&d[2][e]=5");
        }
    }
}
