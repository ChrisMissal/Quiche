# Quiche

A tasty little treat that turns your objects into query strings.

[![chrismissal MyGet Build Status](https://www.myget.org/BuildSource/Badge/chrismissal?identifier=347aee1d-fd73-451d-845b-b2c834150a82)](https://www.myget.org/)

## How to Use Quiche

It's crazy simple to get started, take the following code for example:

```csharp
var builder = new Builder();
var obj = new MixedObject
	{
		ItemNames = new[] { "one", "two" },
		TextMessage = "programming is fun"
	};
var result = builder.ToQueryString(obj);

result.ShouldBe("?ItemNames=one&ItemNames=two&TextMessage=programming+is+fun");
```

### Field Casing Options

Not a fan of Pascal Casing? You can use Camel Case as well.

```csharp
var settings = new BuilderSettings { FieldCasing = FieldCasing.CamelCase };
var builder = new Builder(settings);
var result = builder.ToQueryString(new MixedObject { ItemNames = new[] { "one", "two" }, TextMessage = "programming is fun" });

result.ShouldBe("?itemNames=one&itemNames=two&textMessage=programming+is+fun");
```

### Custom Converter

Maybe Pascal or Camel casing isn't enough for you. You can transform your fields using a custom function like so:

```csharp
var builder = new Builder(x =>
{
    x.FieldCasing = FieldCasing.Custom;
    x.CustomFieldConverter = s => s.ToUpper();
});
var result = builder.ToQueryString(new { Test = "blah" });

result.ShouldBe("?TEST=blah");
```

## Features

[Version 0.0.4](docs/Quiche-0.0.4-features.md)

### License

The MIT License
Copyright (c) 2014 Chris Missal
