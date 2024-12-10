using System.Text.Json.Serialization;

namespace FluentJsonApi.AspNetCore;

public class JsonApiLinks
{
    public string Self { get; set; }
    public string Related { get; set; }
}