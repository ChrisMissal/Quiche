# Quiche

A tasty little treat that turns your objects into query strings.

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

[Version 0.0.1](Quiche-0.0.1-features.md)

### License

The MIT License
Copyright (c) 2014 Chris Missal
