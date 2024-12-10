using System.Text.Json;

namespace FluentJsonApi.AspNetCore.Tests;

class Article
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }
}

public class JsonApiTests
{
    [Fact]
    public void CanBuildJsonApiDocument()
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
        var article = new Article
        {
            Id = 1,
            Title = "JSON:API",
            Content = "JSON:API is a specification for building APIs in JSON",
            Author = "John Doe"
        };

        var document = documentBuilder.BuildDocument(article);
        var json = JsonSerializer.Serialize(document,
            new JsonSerializerOptions()
            {
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

        var jsonDeserialized = JsonSerializer.Deserialize<JsonApiDocument>(json,
            new JsonSerializerOptions()
            {
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

        Assert.Equal(article.Id.ToString(), jsonDeserialized.Data.Id);
    }

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

        var document = documentBuilder.BuildDocument<Article>(articles);
        var json = JsonSerializer.Serialize(document,
            new JsonSerializerOptions()
            {
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

        var jsonDeserialized = JsonSerializer.Deserialize<JsonApiDocumentCollection>(json);
            

        Assert.Equal(articles.Count, jsonDeserialized.Data.Count);
    }
}