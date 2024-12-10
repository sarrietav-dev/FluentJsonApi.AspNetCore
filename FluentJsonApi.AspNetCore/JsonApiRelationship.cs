using System.Text.Json.Serialization;

namespace FluentJsonApi.AspNetCore;

public class JsonApiRelationship
{
    public string Type { get; set; } = string.Empty;
    public JsonApiLinks Links { get; set; } = new();
}