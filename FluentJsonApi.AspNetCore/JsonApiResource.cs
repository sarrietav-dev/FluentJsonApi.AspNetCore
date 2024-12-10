using System.Text.Json.Serialization;

namespace FluentJsonApi.AspNetCore;

public class JsonApiResource
{
    public required string Type { get; set; } = string.Empty;
    public required string Id { get; set; } = string.Empty;
    public Dictionary<string, object> Attributes { get; set; } = new();
    public Dictionary<string, JsonApiRelationship> Relationships { get; set; } = new();
    public JsonApiLinks Links => new() { Self = $"/{Type}/{Id}" };
}