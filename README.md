# FluentJsonApi.AspNetCore

FluentJsonApi.AspNetCore is a .NET library for building JSON:API compliant APIs using a fluent interface. This library simplifies the creation of JSON:API documents and resources, making it easier to adhere to the JSON:API specification.

## Features

- Fluent interface for building JSON:API documents
- Support for defining resource types, attributes, and relationships
- Serialization and deserialization of JSON:API documents
- Integration with ASP.NET Core

## Installation

To install FluentJsonApi.AspNetCore, add the following package reference to your project:

```xml
<PackageReference Include="FluentJsonApi.AspNetCore" Version="1.0.0" />
```

## Usage

### Defining Resources

Define your resources by creating classes and configuring them using the JsonApiBuilder.

```csharp
var jsonApiBuilder = new JsonApiBuilder();
jsonApiBuilder.Entity<Article>()
    .Type("articles")
    .Id(a => a.Id)
    .Attribute(a => a.Title)
    .Attribute(a => a.Content)
    .Relationship(a => a.Author, (a, r) =>
    {
        r.Type = "people";
        r.Links = new JsonApiLinks()
        {
            Self = $"/articles/{a.Id}/relationships/author",
            Related = $"/articles/{a.Id}/author"
        };
    });
```

### Building JSON:API Documents

Use the JsonApiDocumentBuilder to create JSON:API documents.

```csharp
var documentBuilder = new JsonApiDocumentBuilder(jsonApiBuilder);
var article = new Article
{
    Id = 1,
    Title = "JSON:API",
    Content = "JSON:API is a specification for building APIs in JSON",


 Author

 = "John Doe"
};
var document = documentBuilder.BuildDocument(article);
```

### Serializing and Deserializing

Serialize and deserialize JSON:API documents using System.Text.Json.

```csharp
var json = JsonSerializer.Serialize(document, new JsonSerializerOptions
{
    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
});

var deserializedDocument = JsonSerializer.Deserialize<JsonApiDocument>(json);
```

### Unit Testing

Unit tests for the library can be found in the FluentJsonApi.AspNetCore.Tests project. Example test:

```csharp
[Fact]
public void CanBuildJsonApiDocumentCollection()
{
    var jsonApiBuilder = new JsonApiBuilder();
    jsonApiBuilder.Entity<Article>()
        .Type("articles")
        .Id(a => a.Id)
        .Attribute(a => a.Title)
        .Attribute(a => a.Content)
        .Relationship(a => a.Author, (a, r) =>
        {
            r.Type = "people";
            r.Links = new JsonApiLinks()
            {
                Self = $"/articles/{a.Id}/relationships/author",
                Related = $"/articles/{a.Id}/author"
            };
        });

    var documentBuilder = new JsonApiDocumentBuilder(jsonApiBuilder);
    var articles = new List<Article>
    {
        new() {
            Id = 1,
            Title = "JSON:API",
            Content = "JSON:API is a specification for building APIs in JSON",
            Author = "John Doe"
        },
        new() {
            Id = 2,
            Title = "REST",
            Content = "REST (Representational State Transfer) is an architectural style for distributed hypermedia systems",
            Author = "Jane Doe"
        }
    };

    var document = documentBuilder.BuildDocument(articles);
    var json = JsonSerializer.Serialize(document, new JsonSerializerOptions
    {
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    });

    var jsonDeserialized = JsonSerializer.Deserialize<JsonApiDocumentCollection>(json);
    Assert.Equal(articles.Count, jsonDeserialized.Data.Count);
}
```

## License

This project is licensed under the MIT License. See the LICENSE file for details.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request on GitHub.

## Contact

For questions or feedback, please contact the project maintainers.

---

This README provides an overview of the FluentJsonApi.AspNetCore library, including installation instructions, usage examples, and information on unit testing, licensing, and contributing.
