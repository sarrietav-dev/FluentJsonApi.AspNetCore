using System.Text.Json.Serialization;

namespace FluentJsonApi.AspNetCore;

public class JsonApiLinks
{
    public required string Self { get; set; }
    public string? Related { get; set; } = null;
}