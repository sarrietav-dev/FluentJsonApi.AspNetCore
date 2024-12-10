using System.Text.Json.Serialization;

namespace FluentJsonApi.AspNetCore;

public class JsonApiDocumentBuilder(JsonApiBuilder builder)
{

    public JsonApiDocument BuildDocument<TEntity>(TEntity entity) where TEntity : class
    {
        // Check if the entity is a Collection if not then get the type
        var entityType = entity.GetType().IsGenericType ? entity.GetType().GetGenericArguments()[0] : entity.GetType();

        var config = builder.GetConfiguration<TEntity>(entityType);

        var resource = new JsonApiResource()
        {
            Type = config.ResourceType,
            Id = config.IdSelector(entity).ToString() ??
                 throw new InvalidOperationException("IdSelector must return a non-null value.")
        };

        foreach (var attr in config.Attributes)
        {
            resource.Attributes[attr.Key] = attr.Value(entity);
        }

        foreach (var relationship in config.Relationships)
        {
            relationship(entity, resource);
        }

        return new JsonApiDocument { Data = resource };
    }

    public JsonApiDocumentCollection BuildDocument<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
    {
        var config = builder.GetConfiguration<TEntity>(typeof(TEntity));
        var document = new JsonApiDocumentCollection();

        foreach (var entity in entities)
        {
            var resource = new JsonApiResource()
            {
                Type = config.ResourceType,
                Id = config.IdSelector(entity).ToString() ??
                     throw new InvalidOperationException("IdSelector must return a non-null value.")
            };

            foreach (var attr in config.Attributes)
            {
                resource.Attributes[attr.Key] = attr.Value(entity);
            }

            foreach (var relationship in config.Relationships)
            {
                relationship(entity, resource);
            }

            document.Data.Add(resource);
        }

        return document;
    }
}

public class JsonApiDocumentCollection
{
    [JsonPropertyName("data")]
    public List<JsonApiResource> Data { get; } = [];
}

public class JsonApiDocument
{
    [JsonPropertyName("data")]
    public JsonApiResource Data { get; set; }
}