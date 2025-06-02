using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Text.Json.Serialization;

namespace IbeAppWeb.Models;

public class CustomUserAccount : RemoteUserAccount
{
    [JsonPropertyName("roles")] 
    public List<string> Roles { get; set; }
}
