using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Text.Json.Serialization;

namespace IbeAppWeb.Models;

public class CustomUserAccount : RemoteUserAccount
{
    [JsonPropertyName("groups")]
    public List<string> Groups { get; set; }

    [JsonPropertyName("roles")] 
    public List<string> Roles { get; set; }
}
