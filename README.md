# GlobalX.Hypermedia.AspNetCore

GlobalX.Hypermedia.AspNetCore transforms your plain old JSON objects into hypermedia aware response. Currently the only
Using the implemented standard is the [Hal Specification](http://stateless.co/hal_specification.html).

The library inspects the Accept header on requests. If set to `application/json+hal` it will transform the object into
a HAL compliant response accoording to the definitions you provide.

## Installation

Use nuget to pull [Globalx.Hypermedia.AspNetCore](https://www.nuget.org/packages/Globalx.Hypermedia.AspNetCore/) into 
your project.

The add the `HalOutputFormatter` to the set of OutputFormatters to your `AddMvc` declaration.

```c#
public IServiceProvider ConfigureServices(IServiceCollection services)
{
    services.AddMvc(o =>
    {
        o.OutputFormatters.Insert(0, new HalOutputFormatter());   
    }
} 
```

## Usage

To configure your responses create a `HalConfiguration`. For example for a response class;

```c#
public class Recipe {
    public string Name {get; set;}
    public List<string> Ingredients {get; set;}
}
```



```c#
HalConfiguration ConfigureHal()
{
    HalConfiguration hal = new HalConfiguration();
    hal.For<Recipe>()
        .Links(x => new Link("self", "/recipes/{name}"))
        .Links(x => new Link("purchaseIngredients", "/recipes/{name}/purchase"))
    return hal;
}
```

This HalConfiguration is then introduced into the DI system.

```c#
services.AddSingleton<IProvideHalTypeConfiguration>(ConfigureHal());
```

If the accept header is `application/json` then the response will be;

```json
{
  "name": "egg on toast",
  "ingredients": [
    "egg",
    "toast"
  ]
}

```

If the accept header is `application/json+hal` then the response will be;


```json
{
  "name": "egg on toast",
  "ingredients": [
    "egg",
    "toast"
  ],
  "_links": {
    "self": {
        "href": "/recipes/egg+on+toast",
        "templated": false
    },
    "purchaseIngredients": {
        "href": "/recipes/egg+on+toast/purchase",
        "templated": false
    }
  }
}

```

## Publishing

```bash
dotnet pack --include-source --include-symbols -o ./nuget
```


## Credits

This package is essentially a port of Nancy.Hal written by [Dan Barua](https://github.com/danbarua). Modified to
work with AspNet.WebApi.

## Licence

MIT License

Copyright (c) 2019 GlobalX Pty Ltd

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

