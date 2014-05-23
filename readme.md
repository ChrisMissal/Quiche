# Quiche

A tasty little treat that turns your objects into query strings.

[![chrismissal MyGet Build Status](https://www.myget.org/BuildSource/Badge/chrismissal?identifier=347aee1d-fd73-451d-845b-b2c834150a82)](https://www.myget.org/)

## How to Use Quiche

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

## Features

[Version 0.0.2](docs/Quiche-0.0.2-features.md)

### License

The MIT License
Copyright (c) 2014 Chris Missal
