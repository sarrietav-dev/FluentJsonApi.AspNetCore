using System.Linq.Expressions;

namespace FluentJsonApi.AspNetCore;

public class ResourceConfiguration<TEntity> where TEntity : class
{
    public Dictionary<string, Func<TEntity, object>> Attributes { get; } = new();
    public List<Action<TEntity, JsonApiResource>> Relationships { get; } = [];

    public string ResourceType { get; private set; } = null!;
    public Func<TEntity, object> IdSelector { get; private set; } = null!;

    public ResourceConfiguration<TEntity> Type(string type)
    {
        ResourceType = type;
        return this;
    }

    public ResourceConfiguration<TEntity> Attribute<TProperty>(Expression<Func<TEntity, TProperty>> propertySelector)
    {
        var propertyName = ((MemberExpression)propertySelector.Body).Member.Name;
        Attributes[propertyName] = entity =>
            propertySelector.Compile()(entity) ??
            throw new InvalidOperationException($"Attribute '{propertyName}' is null");
        return this;
    }

    public ResourceConfiguration<TEntity> Relationship<TProperty>(Expression<Func<TEntity, TProperty>> propertySelector,
        Action<TEntity, JsonApiRelationship> relationshipConfig)
    {
        Relationships.Add((entity, resource) =>
        {
            var relationship = new JsonApiRelationship();
            relationshipConfig(entity, relationship);
            resource.Relationships.Add(((MemberExpression)propertySelector.Body).Member.Name, relationship);
        });

        return this;
    }

    public ResourceConfiguration<TEntity> Id(Func<TEntity, object> value)
    {
        IdSelector = value;
        return this;
    }
}