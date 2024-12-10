namespace FluentJsonApi.AspNetCore;

public class JsonApiBuilder
{
    private readonly Dictionary<string, object> _resourceConfigurations = new();

    public ResourceConfiguration<TEntity> Entity<TEntity>() where TEntity : class
    {
        var entityType = typeof(TEntity);
        if (_resourceConfigurations.ContainsKey(entityType.Name))
        {
            throw new InvalidOperationException($"Entity {entityType.Name} is already configured.");
        }

        var resourceConfiguration = new ResourceConfiguration<TEntity>();
        _resourceConfigurations.Add(entityType.Name, resourceConfiguration);
        return resourceConfiguration;
    }

    public ResourceConfiguration<TEntity> GetConfiguration<TEntity>(Type? type) where TEntity : class
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        if (!_resourceConfigurations.ContainsKey(type.Name))
        {
            throw new InvalidOperationException($"Entity {type.Name} is not configured.");
        }

        return (ResourceConfiguration<TEntity>)_resourceConfigurations[type.Name];
    }
}